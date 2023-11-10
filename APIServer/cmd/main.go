package main

import (
	"APIServer/internal/db"
	"APIServer/internal/handlers"
	"APIServer/internal/middleware"
	"APIServer/internal/models"
	"log"

	"github.com/gin-gonic/gin"
)

func main() {
	dbase, err := db.OpenDB()
	if err != nil {
		log.Fatal(err)
	}

	// db.AddItemsByConsole(dbase)

	router := gin.Default()

	// auth
	router.GET("/auth", middleware.LoginMiddleware(dbase), handlers.AuthHandler)

	// categories
	router.GET("/categories", middleware.CommonMiddleware(dbase), func(ctx *gin.Context) {
		handlers.GetAllCategoriesHandler(ctx, dbase)
	})

	router.GET("/categories/:category_id", middleware.CommonMiddleware(dbase), func(ctx *gin.Context) {
		handlers.GetCategoryHandler(ctx, dbase)
	})

	router.GET("/categories/:category_id/title", middleware.CommonMiddleware(dbase), func(ctx *gin.Context) {
		handlers.GetCategoryTitle(ctx, dbase)
	})

	router.POST("/categories", middleware.ItemMW[models.Category](dbase), func(ctx *gin.Context) {
		handlers.InsertCategory(ctx, dbase)
	})

	router.DELETE("/categories/:category_id", middleware.CommonMiddleware(dbase), func(ctx *gin.Context) {
		handlers.DeleteCategory(ctx, dbase)
	})

	router.PUT("/categories/:category_id", middleware.ItemMW[models.Category](dbase), func(ctx *gin.Context) {
		handlers.UpdateCategory(ctx, dbase)
	})

	// items
	router.DELETE("/items/:item_id", middleware.CommonMiddleware(dbase), func(ctx *gin.Context) {
		handlers.DeleteItem(ctx, dbase)
	})

	router.POST("/items", middleware.ItemMW[models.Item](dbase), func(ctx *gin.Context) {
		handlers.InsertItem(ctx, dbase)
	})

	router.PUT("/items/:item_id", middleware.ItemMW[models.Item](dbase), func(ctx *gin.Context) {
		handlers.UpdateItem(ctx, dbase)
	})

	router.Run(":8080")
}
