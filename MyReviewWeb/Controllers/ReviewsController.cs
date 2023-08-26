using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyReviewWeb.Data;
using MyReviewWeb.Models;

namespace MyReviewWeb.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
              return _context.Reviews != null ? 
                          View(await _context.Reviews.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Review'  is null.");
        }

        // GET: Reviews/Search
        public async Task<IActionResult> Search()
        {
            return View();
        }

        // POST: Reviews/SearchResults
        public async Task<IActionResult> SearchResults(string SearchReview)
        {
            return _context.Reviews != null ?
                         View("Index", await _context.Reviews.Where(j => j.Link.Contains(SearchReview) 
                         || j.Title.Contains(SearchReview) || j.Text.Contains(SearchReview)).ToListAsync()) :
                         Problem("Entity set 'ApplicationDbContext.Review'  is null.");

        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            if (ModelState.IsValid)
            {
                review.User = User.Identity.Name;
                _context.Add(review);
                await _context.SaveChangesAsync();
                TempData["success"] = "Review created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        [Authorize]
        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reviews == null )
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text,Link")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                try
                {
                    review.User = User.Identity.Name;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Review updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }


        [Authorize]
        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);

        }


        // POST: Reviews/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Review'  is null.");
            }

            var review = await _context.Reviews.FindAsync(id);
            
            if (review != null && ModelState.IsValid)
            {
                _context.Reviews.Remove(review);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Review deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
          return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        // POST: Reviews/Index/Like
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int id)
        {
            if ( _context.Reviews == null)
            {
                return NotFound();
            }

            Review review = await _context.Reviews.FirstOrDefaultAsync(m => m.Id == id);
            
            Like like = new Like();
            like.IdReview = id;
            like.User = User.Identity.Name;
            

            if (review == null )
            {
                return NotFound();
            }

            if (ModelState.IsValid && !Liked(id))
            {
                review.LikesCount += 1;

                _context.Update(review);
                _context.Add(like);
                await _context.SaveChangesAsync();

               
            }
            
            return RedirectToAction(nameof(Index));
        }

        //GET 
        private bool Liked(int id)
        {
            List<Like>? likes = _context.Likes.Where(j => j.IdReview == id).ToList();
            List<string> usersLiked = likes.Select(j => j.User).ToList();
            
            return usersLiked.Contains(User.Identity.Name);

            
        }

    }

}

