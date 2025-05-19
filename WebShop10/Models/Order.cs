using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop10.Models
{
    public class Order
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Datum narudžbe")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Total is required!")]
        [Column(TypeName = "decimal(9, 2)")]
        [DisplayName("Ukupni iznos narudžbe")]
        public decimal Total { get; set; }

        public string UserId { get; set; } = null!;

        #region Shipping info...

        [Required(ErrorMessage = "First name is required!")]
        [StringLength(50)]
        public string ShippingFirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required!")]
        [StringLength(50)]
        public string ShippingLastName { get; set; } = null!;

        [Required(ErrorMessage = "Address is required!")]
        [StringLength(50)]
        public string ShippingAddress { get; set; } = null!;

        [Required(ErrorMessage = "Zipcode is required!")]
        [StringLength(10)]
        public string ShippingZipCode { get; set; } = null!;

        [Required(ErrorMessage = "City is required!")]
        [StringLength(50)]
        public string ShippingCity { get; set; } = null!;

        [Required(ErrorMessage = "Country is required!")]
        [StringLength(50)]
        public string ShippingCountry { get; set; } = null!;

        [Required(ErrorMessage = "E-mail address is required!")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail address is not valid")]
        public string ShippingEmail { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required!")]
        [StringLength(20)]
        public string ShippingPhone { get; set; } = null!;

        #endregion

        #region Billing info...

        [StringLength(50)]
        public string? BillingFirstName { get; set; }

        [StringLength(50)]
        public string? BillingLastName { get; set; }

        [StringLength(50)]
        public string? BillingAddress { get; set; }

        [StringLength(10)]
        public string? BillingZipCode { get; set; }

        [StringLength(50)]
        public string? BillingCity { get; set; }

        [StringLength(50)]
        public string? BillingCountry { get; set; }

        [StringLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail address is not valid")]
        public string? BillingEmail { get; set; }

        [StringLength(20)]
        public string? BillingPhone { get; set; }

        #endregion

        [NotMapped]
        public bool UseSameAddress { get; set; } = true;

        public string? Message { get; set; }

        [ForeignKey("OrderId")]
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
