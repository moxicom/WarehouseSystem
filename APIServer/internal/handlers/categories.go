package handlers

import (
	"APIServer/internal/db"
	"APIServer/internal/models"
	"database/sql"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

// endpoint: /categories
func GetAllCategoriesHandler(ctx *gin.Context, dbase *sql.DB) {
	categories, err := db.GetAllCategories(dbase)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, categories)
}

// endpoint: /categories/:category_id
func GetCategoryHandler(ctx *gin.Context, dbase *sql.DB) {
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
}

// endpoint: /categories/:category_id
func DeleteCategory(ctx *gin.Context, dbase *sql.DB) {
	categoryIDstr := ctx.Param("category_id")
	categoryIDInt, err := strconv.Atoi(categoryIDstr)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	err = db.DeleteItemsByCategory(dbase, categoryIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	err = db.DeleteCategory(dbase, categoryIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.Status(http.StatusOK)
}

// endpoint: /categories/:category_id/title
func GetCategoryTitle(ctx *gin.Context, dbase *sql.DB) {
	categoryIDstr := ctx.Param("category_id")
	categoryIDInt, err := strconv.Atoi(categoryIDstr)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	title, err := db.GetCategoryTitle(dbase, categoryIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, title)
}

// endpoint: /categories
func InsertCategory(ctx *gin.Context, dbase *sql.DB) {
	itemCtx, _ := ctx.Get("item")
	item, ok := itemCtx.(models.Category)
	if !ok {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	userCtx, _ := ctx.Get("user")
	user, ok := userCtx.(models.User)
	if !ok {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	if err := db.InsertCategory(dbase, item, user.ID); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

// endpoint: /categories/:category_id
func UpdateCategory(ctx *gin.Context, dbase *sql.DB) {
	itemCtx, _ := ctx.Get("item")
	item, ok := itemCtx.(models.Category)
	if !ok {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}

	if err := db.UpdateCategory(dbase, item); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}
