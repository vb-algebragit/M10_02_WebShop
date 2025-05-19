using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebShop10.Models;

namespace WebShop10.Data
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [StringLength(150)]
        public string Address { get; set; } = null!;

        [ForeignKey("UserId")]
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
