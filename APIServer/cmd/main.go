package main

import (
	"APIServer/internal/db"
	"APIServer/internal/middleware"
	"APIServer/internal/models"
	"log"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

func main() {
	dbase, err := db.OpenDB()
	if err != nil {
		log.Fatal(err)
	}

	// db.AddItemsByConsole(dbase)

	router := gin.Default()

	router.GET("/auth", middleware.LoginMiddleware(dbase), func(ctx *gin.Context) {
		user, _ := ctx.Get("user")
		ctx.JSON(http.StatusOK, user)
	})

	router.GET("/categories", middleware.CommonMiddleware(dbase), func(ctx *gin.Context) {
		categories, err := db.GetAllCategories(dbase)
		if err != nil {
			ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		}
		ctx.JSON(http.StatusOK, categories)
	})

	router.GET("/categories/:category_id", middleware.CommonMiddleware(dbase), func(ctx *gin.Context) {
		categoryIDstr := ctx.Param("category_id")
		categoryIDInt, err := strconv.Atoi(categoryIDstr)
		if err != nil {
			ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
			return
		}

		items, err := db.GetAllItems(dbase, categoryIDInt)
		if err != nil {
			ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
			return
		}
		title, err := db.GetCategoryTitle(dbase, categoryIDInt)

		if err != nil {
			ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		}

		type Ans struct {
			Title string        `json:"Title"`
			Items []models.Item `json:"Items"`
		}

		data := Ans{title, items}

		ctx.JSON(http.StatusOK, data)
	})

	router.DELETE("/items/:item_id", middleware.CommonMiddleware(dbase), func(ctx *gin.Context) {
		itemIDstr := ctx.Param("item_id")
		itemIDInt, err := strconv.Atoi(itemIDstr)
		if err != nil {
			ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
			return
		}

		err = db.DeleteItem(dbase, itemIDInt)
		if err != nil {
			ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
			return
		}
		ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
	})

	router.POST("/items", middleware.ItemMW(dbase), func(ctx *gin.Context) {
		itemCtx, _ := ctx.Get("item")
		item, ok := itemCtx.(models.Item)
		if !ok {
			ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
			return
		}
		if err := db.InsertItem(dbase, item); err != nil {
			ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		}
		ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
	})
	router.Run(":8080")
}
