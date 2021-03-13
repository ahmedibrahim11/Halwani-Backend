using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Halwani
{
    public static class ServiceConfiguration
    {
        public static void SwaggerConfiguration(this IServiceCollection services)
        {
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,

					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme.",
				}); ;
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							},
							new string[] {}

					}
				});
			});

		}
	}
}
