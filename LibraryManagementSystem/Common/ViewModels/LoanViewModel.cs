using System.Collections.Generic;
using Common.DTOs;

// En Common/ViewModels/LoanViewModel.cs
namespace Common.ViewModels
{
    public class LoanViewModel
    {
        public List<UserDTO> Users { get; set; }
        public List<BookDTO> Books { get; set; }
    }
}

