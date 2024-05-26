using KnowledgeSpace.ViewModels;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeSpace.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // [POST]: http://5001/api/roles
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel roleViewModel)
        {
            var role = new IdentityRole()
            {
                Name = roleViewModel.Name,
                NormalizedName = roleViewModel.Name.ToUpper()
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
            }

            return BadRequest(result.Errors);
        }

        // [GET] http://5001/api/roles
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleManager.Roles.Select(r => new RoleViewModel()
            {
                Id = r.Id,
                Name = r.Name
            }).ToListAsync();

            return Ok(roles);
        }


        // [GET] http://5001/api/roles?keyword={keyword}&pageIndex=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetRolesPaging(string keyword, int pageIndex, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Id.Contains(keyword));
            }

            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            var pagination = new Pagination<RoleViewModel>
            {
                Data = items,
                TotalRecords = totalRecords,
            };

            return Ok(pagination);
        }


        // [GET]: http://5001/api/roles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var roleViewModel = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };

            return Ok(roleViewModel);
        }

        // [PUT]: http://5001/api/roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleViewModel roleViewModel)
        {
            if (id != roleViewModel.Id)
            {
                return BadRequest();
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            role.Name = roleViewModel.Name;
            role.NormalizedName = roleViewModel.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        // [DELETE]: http://5001/api/roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                var roleViewModel = new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name
                };

                return Ok(roleViewModel);
            }

            return BadRequest(result.Errors);
        }   
    }
}
