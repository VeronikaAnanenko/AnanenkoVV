using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSUIR.BL.Interfaces.Models;
using BSUIR.BL.Interfaces.Models.Categories;
using BSUIR.BL.Interfaces.Models.Items;
using BSUIR.BL.Interfaces.Models.Photos;
using BSUIR.BL.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BSUIR.Web.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemService _itemService;
        private readonly ICategoryService _categoryService;
        private readonly IPhotoService _photoService;
        private readonly List<string> Types = new List<string>();
        private readonly List<string> Colors = new List<string>();
        private readonly List<string> Producers = new List<string>();
        private readonly List<string> Materials = new List<string>();
        private readonly List<string> Countries = new List<string>();
        private Basket _basket;
        public ItemsController(IItemService itemService, IPhotoService photoService, Basket basket, ICategoryService categoryService)
        {
            _itemService = itemService;
            _photoService = photoService;
            this._basket = basket;
            _categoryService = categoryService;
        }
        [HttpPost]
        public async Task<IActionResult> Index(int id, string placeholder)
        {
            var basketItem = await _itemService.GetItemByIdAsync<Item>(id);
            ViewData["CategoryId"] = id;
            var basketPhotos = await _photoService.GetRelatedPhotosAsync<Photo>(basketItem.Id);
            basketItem.Link = basketPhotos.ToList().ElementAt(0).Link;
            _basket.Items.Add(basketItem);
            _basket.Price += basketItem.Price;
            var items = await _itemService.GetItemsAsync<Item>();
            var result = items.Where(x => x.CategoryId == basketItem.CategoryId).ToList();
            foreach (var item in result)
            {
                var photos = await _photoService.GetRelatedPhotosAsync<Photo>(item.Id);

                item.Link = photos.ToList().ElementAt(0).Link;
                if (!Types.Contains(item.Type))
                    Types.Add(item.Type);
                if (!Colors.Contains(item.Color))
                    Colors.Add(item.Color);
                if (!Producers.Contains(item.Producer))
                    Producers.Add(item.Producer);
                if (!Materials.Contains(item.Material))
                    Materials.Add(item.Material);
                if (!Countries.Contains(item.Country))
                    Countries.Add(item.Country);
            }

            ViewData["Types"] = Types;
            ViewData["Colors"] = Colors;
            ViewData["Producers"] = Producers;
            ViewData["Materials"] = Materials;
            ViewData["Countries"] = Countries;
            return View(result);
        }
        public async Task<IActionResult> Index(int id)
        {
            var items = await _itemService.GetItemsAsync<Item>();
            var result = items.Where(x => x.CategoryId == id).ToList();
            ViewData["CategoryId"] = id;
            foreach (var item in result)
            {
                var photos = await _photoService.GetRelatedPhotosAsync<Photo>(item.Id);

                item.Link = photos.ToList().ElementAt(0).Link;
                if (!Types.Contains(item.Type))
                    Types.Add(item.Type);
                if (!Colors.Contains(item.Color))
                    Colors.Add(item.Color);
                if (!Producers.Contains(item.Producer))
                    Producers.Add(item.Producer);
                if (!Materials.Contains(item.Material))
                    Materials.Add(item.Material);
                if (!Countries.Contains(item.Country))
                    Countries.Add(item.Country);
            }

            ViewData["Types"] = Types;
            ViewData["Colors"] = Colors;
            ViewData["Producers"] = Producers;
            ViewData["Materials"] = Materials;
            ViewData["Countries"] = Countries;
            return View(result);
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateCategory(string Category)
        {
            var category = await _categoryService.CreateCategoryAsync<Category>(new Category()
            {
                Name = Category
            });
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public async Task<IActionResult> CreateItem(int id)
        {
            var item = new Item();
            item.CategoryId = id;
            return View("Create", item);
        }
        [HttpPost]
        public async Task<IActionResult> CreateItem(Item item)
        {
            item.Date = DateTime.Now;
            item.Id = 0;
            var created = await _itemService.CreateItemAsync<Item>(item);
            var photo = new Photo()
            {
                ItemId = created.Id,
                Link = "http://ovkom.ru/wp-content/uploads/2019/08/placeholder.png",
                Name = "Placeholder"
            };
            await _photoService.CreatePhotoAsync<Photo>(photo);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> AddPhoto(string Link, int Id)
        {
            var item = await _itemService.GetItemByIdAsync<Item>(Id);
            var photos = await _photoService.GetRelatedPhotosAsync<Photo>(Id);
            if (photos.ElementAt(0).Name == "Placeholder")
            {
                await _photoService.DeletePhotoAsync(photos.ElementAt(0).Id);
            }

            await _photoService.CreatePhotoAsync<Photo>(new Photo()
            {
                ItemId = item.Id,
                Link = Link
            });
            photos = await _photoService.GetRelatedPhotosAsync<Photo>(Id);
            ViewData["Photos"] = photos;
            return View("Details", item);
        }

        public async Task<IActionResult> Sort(int id, string Sort)
        {
            var items = await _itemService.GetItemsAsync<Item>();
            var result = items.Where(x => x.CategoryId == id).ToList();
            if (Sort == "Дорогие")
            {
                result = result.OrderByDescending(x => x.Price).ToList();
            }
            if (Sort == "Дешёвые")
            {
                result = result.OrderBy(x => x.Price).ToList();
            }
            if (Sort == "Новые")
            {
                result = result.OrderBy(x => x.Date).ToList();
            }
            ViewData["CategoryId"] = id;
            foreach (var item in result)
            {
                var photos = await _photoService.GetRelatedPhotosAsync<Photo>(item.Id);

                item.Link = photos.ToList().ElementAt(0).Link;
                if (!Types.Contains(item.Type))
                    Types.Add(item.Type);
                if (!Colors.Contains(item.Color))
                    Colors.Add(item.Color);
                if (!Producers.Contains(item.Producer))
                    Producers.Add(item.Producer);
                if (!Materials.Contains(item.Material))
                    Materials.Add(item.Material);
                if (!Countries.Contains(item.Country))
                    Countries.Add(item.Country);
            }

            ViewData["Types"] = Types;
            ViewData["Colors"] = Colors;
            ViewData["Producers"] = Producers;
            ViewData["Materials"] = Materials;
            ViewData["Countries"] = Countries;
            return View("Index", result);
        }

        public async Task<IActionResult> Filter(string Filter, int id)
        {
            var items = await _itemService.GetItemsAsync<Item>();
            var result = items.Where(x => x.CategoryId == id).ToList();
            var filters = Filter.Split("|");
            var types = filters[0].Split(',');
            var materials = filters[1].Split(',');
            var colors = filters[2].Split(',');
            var producers = filters[3].Split(',');
            var countries = filters[4].Split(',');
            var widths = filters[5].Split(',');
            var prices = filters[6].Split(',');
            var heights = filters[7].Split(',');
            var weights = filters[8].Split(',');
            var lengths = filters[9].Split(',');
            var powers = filters[10].Split(',');
            if (types.Count() != 0)
            {
                result = result.Where(x => types.Contains(x.Type)).ToList();
            }

            if (materials.Count() != 0)
            {
                result = result.Where(x => materials.Contains(x.Material)).ToList();

            }

            if (colors.Count() != 0)
            {
                result = result.Where(x => colors.Contains(x.Color)).ToList();
            }
            if (producers.Count() != 0)
            {
                result = result.Where(x => producers.Contains(x.Producer)).ToList();
            }
            if (countries.Count() != 0)
            {
                result = result.Where(x => countries.Contains(x.Country)).ToList();
            }
            if (widths.Count() > 1)
            {
                result = result.Where(x => x.Width>Convert.ToDecimal(widths[0])&& x.Width < Convert.ToDecimal(widths[1])).ToList();
            }                              
            if (prices.Count() > 1)       
            {                              
                result = result.Where(x => x.Price> Convert.ToDecimal(prices[0]) && x.Price < Convert.ToDecimal(prices[1])).ToList();
            }                              
            if (heights.Count() > 1)      
            {                              
                result = result.Where(x => x.Height > Convert.ToDecimal(heights[0]) && x.Height< Convert.ToDecimal(heights[1])).ToList();
            }                              
            if (weights.Count() > 1)      
            {                              
                result = result.Where(x => x.Weight> Convert.ToDecimal(weights[0]) && x.Weight < Convert.ToDecimal(weights[1])).ToList();
            }                              
            if (lengths.Count() > 1 )      
            {                              
                result = result.Where(x => x.Length > Convert.ToDecimal(lengths[0]) && x.Length< Convert.ToDecimal(lengths[1])).ToList();
            }                              
            if (powers.Count() > 1)       
            {                              
                result = result.Where(x => x.Power > Convert.ToDecimal(powers[0]) && x.Power < Convert.ToDecimal(powers[1])).ToList();
            }
            ViewData["CategoryId"] = id;
            foreach (var item in result)
            {
                var photos = await _photoService.GetRelatedPhotosAsync<Photo>(item.Id);

                item.Link = photos.ToList().ElementAt(0).Link;
                if (!Types.Contains(item.Type))
                    Types.Add(item.Type);
                if (!Colors.Contains(item.Color))
                    Colors.Add(item.Color);
                if (!Producers.Contains(item.Producer))
                    Producers.Add(item.Producer);
                if (!Materials.Contains(item.Material))
                    Materials.Add(item.Material);
                if (!Countries.Contains(item.Country))
                    Countries.Add(item.Country);
            }

            ViewData["Types"] = Types;
            ViewData["Colors"] = Colors;
            ViewData["Producers"] = Producers;
            ViewData["Materials"] = Materials;
            ViewData["Countries"] = Countries;
            return View("Index", result);
        }
        public async Task<IActionResult> Search(string SearchString)
        {
            var items = await _itemService.GetItemsAsync<Item>();
            var result = items.Where(x => x.Name.Contains(SearchString)).ToList();
            foreach (var item in result)
            {
                var photos = await _photoService.GetRelatedPhotosAsync<Photo>(item.Id);

                item.Link = photos.ToList().ElementAt(0).Link;
                if (!Types.Contains(item.Type))
                    Types.Add(item.Type);
                if (!Colors.Contains(item.Color))
                    Colors.Add(item.Color);
                if (!Producers.Contains(item.Producer))
                    Producers.Add(item.Producer);
                if (!Materials.Contains(item.Material))
                    Materials.Add(item.Material);
                if (!Countries.Contains(item.Country))
                    Countries.Add(item.Country);
            }

            ViewData["Types"] = Types;
            ViewData["Colors"] = Colors;
            ViewData["Producers"] = Producers;
            ViewData["Materials"] = Materials;
            ViewData["Countries"] = Countries;
            return View("Index", result);
        }
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _itemService.DeleteItemAsync(id);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Item item)
        {
            item.Date = DateTime.Now;
            await _itemService.UpdateItemAsync<Item>(item);
            return RedirectToAction("Details", "Items", new { id = item.Id });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemService.GetItemByIdAsync<Item>(id);
            var photos = await _photoService.GetRelatedPhotosAsync<Photo>(id);
            ViewData["Photos"] = photos;
            return View(item);
        }
        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemService.GetItemByIdAsync<Item>(id);
            var photos = await _photoService.GetRelatedPhotosAsync<Photo>(id);
            ViewData["Photos"] = photos;
            return View(item);
        }
    }
}