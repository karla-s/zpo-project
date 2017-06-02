﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fitshop.Models
{
    public class FoodModel
    {
        public int? UserId { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "food name")]
        public string FoodName { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name = "protein")]
        public double? Protein { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name = "carbs")]
        public double? Carbs { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name = "fat")]
        public double? Fat { get; set; }

        [Range(0.0, 10000.0, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name = "calories")]
        public double? Calories { get; set; }

        public static food Update(food foodToUpdate, FoodModel dataToUpdate)
        {
            foodToUpdate.userId = dataToUpdate.UserId ?? foodToUpdate.userId;
            foodToUpdate.Calories = dataToUpdate.Calories ?? foodToUpdate.Calories;
            foodToUpdate.Carbs = dataToUpdate.Carbs ?? foodToUpdate.Carbs;
            foodToUpdate.Fat = dataToUpdate.Fat ?? foodToUpdate.Fat;
            foodToUpdate.foodName = dataToUpdate.FoodName ?? foodToUpdate.foodName;
            foodToUpdate.Protein = dataToUpdate.Protein ?? foodToUpdate.Protein;

            return foodToUpdate;
        }
    }
}