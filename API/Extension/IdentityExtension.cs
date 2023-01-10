using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API.Extension
{
    public static class IdentityExtension
    {
        public static IServiceCollection EntityServiceExtension(this IServiceCollection services,
            IConfiguration configuration)
        {
            
            //Authentication
            services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme
            )
            // Adding Jwt Bearer
            .AddJwtBearer(options => {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters() {
                    ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidAudience = configuration["Auth:Audience"],
                        ValidIssuer = configuration["Auth:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]))
                };
            });

            
            
        return services;
        }
    }
}