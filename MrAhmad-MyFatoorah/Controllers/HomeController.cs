using Microsoft.AspNetCore.Mvc;
using MrAhmad_MyFatoorah.Services;

namespace MrAhmad_MyFatoorah.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> SendPIN(string phoneNumber)
        {
            //Phone Validate
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest($"{phoneNumber} Is Null");

            //Create Random Code
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var code =  new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            //Send And Save Code To Phone Number
            try
            {
                BulkSMS.SendMessage(phoneNumber + "," + code);
                var getcotext = Config.GetContext();
                await getcotext.ValidationCodes.AddAsync(new()
                {
                    PhoneNumber = phoneNumber,
                    Code = code,
                    SendDate = DateTime.Now
                });
                await getcotext.SaveChangesAsync();
                return Ok();
            }

            //If throw Error
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> CheckPIN(string phoneNumber , string PIN)
        {
            //Phone And PIN Validate
            if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(PIN))
                return BadRequest($"{phoneNumber} or {PIN}  Is Null");

            try
            {
                var getcotext = Config.GetContext();
                var numberOfAttempts = getcotext.Attempts.Where(x => x.PhoneNumber == phoneNumber).Count();
                if (numberOfAttempts > 5)
                    return BadRequest($"You have tried several times, please try again later");

                var checkCodes = getcotext.ValidationCodes.Where(x=>x.Code == PIN && x.PhoneNumber == phoneNumber);
                if(!checkCodes.Any())
                {
                    await getcotext.Attempts.AddAsync(new()
                    {
                        PhoneNumber = phoneNumber,
                        TryDate = DateTime.Now
                    });
                    await getcotext.SaveChangesAsync();
                    return BadRequest($"The entered code is invalid");
                }

                if (checkCodes.Max(x => x.SendDate) > DateTime.Now.AddMinutes(-15))
                    return BadRequest($"The code has expired");

                getcotext.Attempts.RemoveRange(getcotext.Attempts.Where(x => x.PhoneNumber == phoneNumber));
                await getcotext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
