package handlers

import (
	"APIServer/internal/models"
	"log"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

type itemData struct {
	UserID int         `json:"userID"`
	Data   models.Item `json:"data"`
}

func (h *handler) deleteItem(ctx *gin.Context) {
	itemID, err := strconv.Atoi(ctx.Param("item_id"))
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	err = h.r.DeleteItem(itemID)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

func (h *handler) insertItem(ctx *gin.Context) {
	var input itemData
	if err := ctx.ShouldBindJSON(&input); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	log.Println(input)
	if err := h.r.InsertItem(input.Data); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

// endpoint: /items/:item_id
func (h *handler) updateItem(ctx *gin.Context) {
	var input itemData
	if err := ctx.ShouldBindJSON(&input); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	itemID, err := strconv.Atoi(ctx.Param("item_id"))
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	input.Data.ID = itemID
	log.Println(input)
	if err := h.r.UpdateItem(input.Data); err != nil {
		log.Println("Ошибка обновления записи в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}
