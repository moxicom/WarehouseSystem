package handlers

import (
	"APIServer/internal/models"
	"log"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

// endpoint: /categories
func (h *handler) getAllCategoriesHandler(ctx *gin.Context) {
	categories, err := h.r.GetAllCategories()
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, categories)
}

// endpoint: /categories/:category_id
func (h *handler) getCategoryHandler(ctx *gin.Context) {
	categoryIDInt, err := strconv.Atoi(ctx.Param("category_id"))
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	items, err := h.r.GetAllItems(categoryIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}

	ctx.JSON(http.StatusOK, items)
}

// endpoint: /categories/:category_id
func (h *handler) deleteCategory(ctx *gin.Context) {
	categoryIDstr := ctx.Param("category_id")
	categoryIDInt, err := strconv.Atoi(categoryIDstr)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	err = h.r.DeleteItemsByCategory(categoryIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	err = h.r.DeleteCategory(categoryIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.Status(http.StatusOK)
}

// endpoint: /categories/:category_id/title
func (h *handler) getCategoryTitle(ctx *gin.Context) {
	categoryIDstr := ctx.Param("category_id")
	categoryIDInt, err := strconv.Atoi(categoryIDstr)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	title, err := h.r.GetCategoryTitle(categoryIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, title)
}

// endpoint: /categories
func (h *handler) insertCategory(ctx *gin.Context) {
	itemCtx, _ := ctx.Get("data")
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
	if err := h.r.InsertCategory(item, user.ID); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

// endpoint: /categories/:category_id
func (h *handler) updateCategory(ctx *gin.Context) {
	itemCtx, _ := ctx.Get("data")
	item, ok := itemCtx.(models.Category)
	if !ok {
		log.Println("Не удалось распарсить")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	if err := h.r.UpdateCategory(item); err != nil {
		log.Println("Ошибка обновления записи в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}
