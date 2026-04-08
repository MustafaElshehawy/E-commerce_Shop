using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;
using Utilities;

namespace Tasneem_Shop.DataAccess.DbInitializar
{
    public class DbInitializar:IDbInitializar
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializar(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            //Migration 
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {

                    _context.Database.Migrate();
                }

            }
            catch (Exception)
            {

                throw;
            }

            //Role

            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();



                //User

                _userManager.CreateAsync(new ApplicationUser

                { 
                    UserName="Admin@Tasneem.com",
                    Email = "Admin@Tasneem.com",
                    Name ="Administrator",
                    PhoneNumber="012345678",
                    Address="Mansoura",
                    City="Alex",
                },"ADMin123@@").GetAwaiter().GetResult();

                ApplicationUser user =_context.ApplicationUsers.FirstOrDefault(u=>u.Email == "Admin@Tasneem.com");

                _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();

            }



            return;

        }

    }


}
