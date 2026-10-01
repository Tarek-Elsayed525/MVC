using mvc.Models;

namespace mvc.ViewModel
{
    public class PersonVM
    {
        public IEnumerable<Person> persons { get; set; }
        public int count { get; set; }
    }
}
