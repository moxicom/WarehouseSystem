package main

import (
	"APIServer/internal/db"
	"APIServer/internal/middleware"
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
		ctx.JSON(http.StatusOK, items)
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

	router.Run(":8080")
}
