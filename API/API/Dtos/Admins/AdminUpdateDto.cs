namespace API.Dtos.Admins
{
    public class AdminUpdateDto
    {
        public Guid AdminId { get; set; }

        public string AdminUserName { get; set; }


        public string AdminFirstName { get; set; }


        public string AdminLastName { get; set; }

        public string AdminPassword { get; set; }
        public string AdminEmail { get; set; }

        public string AdminPhoneNumber { get; set; }

        public string AdminAddress { get; set; }
    }
}
