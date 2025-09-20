using BulkyWebRazor_Temp.Data1;
using BulkyWebRazor_Temp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {

        public readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int id)
        {
            Category = _db.Categories.Find(id);

        }
        public IActionResult OnPost()
        {
            //if (ModelState.IsValid)
            //{
                _db.Categories.Remove(Category);
                _db.SaveChanges();
            TempData["Success"] = "Category Deleted Successfully";
            return RedirectToPage("Index");
            //}
            return Page();
        }
    }
}
