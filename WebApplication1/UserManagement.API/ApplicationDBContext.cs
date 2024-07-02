namespace UserManagement.API
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Microsoft.AspNetCore.Identity;


    [Flags]
    public enum Permissions
    {
        None = 0,
        ManageDirectTax = 1 << 0,
        ManageInDirectTax = 1 << 1,
        ManageCompliance = 1 << 2,
        DirectTaxReadOnly = 1 << 3,
        DirectTaxAddOnly = 1 << 4,
        DirectTaxModifyOnly = 1 << 5,
        DirectTaxUploadOnly = 1 << 6,
        DirectTaxDeleteOnly = 1 << 7,
        InDirectTaxReadOnly = 1 << 8,
        InDirectTaxAddOnly = 1 << 9,
        InDirectTaxModifyOnly = 1 << 10,
        InDirectTaxUploadOnly = 1 << 11,
        InDirectTaxDeleteOnly = 1 << 12,
        ComplianceTaxReadOnly = 1 << 13,
        ComplianceTaxAddOnly = 1 << 14,
        ComplianceTaxModifyOnly = 1 << 15,
        ComplianceTaxUploadOnly = 1 << 16,
        ComplianceDeleteOnly = 1 << 17
    }

    public class ApplicationUser : IdentityUser
    {
        public int TenantId { get; set; }
        public string Role { get; set; }
        public Permissions UserPermissions { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }
        public ApplicationRole(string role):base(role) { }
        //public string Description { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
        }
    }

}
