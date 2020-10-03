using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NW
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var file = CheckSecurityKey("security-key.txt");

            /** security key is stored in a text file. If it was stored in mongdo repository 
            it would reveal the path's existence to unauthorized users was the repository offline **/
            var hashed = BCrypt.Net.BCrypt.EnhancedHashPassword(File.ReadAllText("security-key.txt"));

            httpContext.Request.Headers.TryGetValue("Authorization", out var value);
            string token = value.ToString();

            /** b-crypt is used to verify the token because a regular string comparison returns on the first 
            unmatching character, revealing the number of matching characters before the umatching one **/
            if (token.Length == 0 || !BCrypt.Net.BCrypt.EnhancedVerify(token, hashed))
            {
                httpContext.Response.StatusCode = 404;
                return;
            }

            await _next(httpContext);
        }

        public string CheckSecurityKey(string file)
        {
            if (!File.Exists(file))
                using (StreamWriter sw = File.AppendText(file)) sw.Write(GenerateRandomKey());

            return file;
        }

        public string GenerateRandomKey()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/\\%&#$£|{}[]()\'\"+-:;";
            var key = new char[50];
            var random = new Random();

            for (int i = 0; i < key.Length; i++)
                key[i] = chars[random.Next(chars.Length)];

            return new string(key);
        }
    }
}