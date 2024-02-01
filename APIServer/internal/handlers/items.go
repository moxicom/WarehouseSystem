package handlers

import (
	"APIServer/internal/models"
	"log"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

func (h *handler) deleteItem(ctx *gin.Context) {
	itemIDstr := ctx.Param("item_id")
	itemIDInt, err := strconv.Atoi(itemIDstr)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	err = h.r.DeleteItem(itemIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

func (h *handler) insertItem(ctx *gin.Context) {
	itemCtx, _ := ctx.Get("data")
	item, ok := itemCtx.(models.Item)
	if !ok {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	if err := h.r.InsertItem(item); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

// endpoint: /items/:item_id
func (h *handler) updateItem(ctx *gin.Context) {
	itemCtx, _ := ctx.Get("data")
	item, ok := itemCtx.(models.Item)
	if !ok {
		log.Println("Не удалось распарсить")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	if err := h.r.UpdateItem(item); err != nil {
		log.Println("Ошибка обновления записи в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}
