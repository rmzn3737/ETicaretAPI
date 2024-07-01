﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETicaret.Application.Abstractions.Services;
using ETicaretAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Persistence.Services
{
    public class RoleService:IRoleService
    {
        readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public (object, int) GetAllRoles(int page, int size)
        {
            var query = _roleManager.Roles;

            return (query.Skip(page * size).Take(size).Select(r => new { r.Id, r.Name }), query.Count());
        }

        public async Task<(string id, string name)> GetRoleById(string id)
        {
            string role = await _roleManager.GetRoleIdAsync(new() { Id = id });
            return (id, role);
        }

        public async Task<bool> CreateRole(string name)
        {
            IdentityResult result = await _roleManager.CreateAsync(new()
            {
                Name = name
            });
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string name)
        {
            IdentityResult result = await _roleManager.DeleteAsync(new() { Name = name });
            return result.Succeeded;
        }

        public async Task<bool> UpdateRole(string id,string name)
        {
            IdentityResult result = await _roleManager.UpdateAsync(new() {Id = id, Name = name});
            return result.Succeeded;
        }
    }
}
