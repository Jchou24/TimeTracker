using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TimeTracker.Helper.Extensions
{
    public static class SessionAuthExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// return null if not log in
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            string idString = claimsPrincipal.FindFirstValue(ClaimTypes.Sid);
            int id = -1;

            if (!Int32.TryParse(idString, out id))
            {
                return null;
            }

            return id;
        }
    }
}
