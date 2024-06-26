﻿using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;

namespace IdentityServer;

public static class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (userMgr.Users.Any())
        {
            Log.Information("THERE IS NO NEED TO SEED DATABASE");

            return;
        }

        var alice = userMgr.FindByNameAsync("alice").Result;

        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
            };

            var result = userMgr.CreateAsync(alice, "Pass123$").Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(alice, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
            }).Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("ALICE CREATED");
        }
        else
        {
            Log.Debug("ALICE ALREADY EXISTS");
        }

        var bob = userMgr.FindByNameAsync("bob").Result;

        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true
            };

            var result = userMgr.CreateAsync(bob, "Pass123$").Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(bob, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Bob Smith"),
                new Claim(JwtClaimTypes.GivenName, "Bob"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.WebSite, "http://bob.com")
            }).Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Log.Debug("BOB CREATED");
        }
        else
        {
            Log.Debug("BOB ALREADY EXISTS");
        }
    }
}