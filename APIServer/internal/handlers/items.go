package handlers

import (
	"APIServer/internal/db"
	"APIServer/internal/models"
	"database/sql"
	"log"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

func DeleteItem(ctx *gin.Context, dbase *sql.DB) {
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
}

func InsertItem(ctx *gin.Context, dbase *sql.DB) {
	itemCtx, _ := ctx.Get("item")
	item, ok := itemCtx.(models.Item)
	if !ok {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	if err := db.InsertItem(dbase, item); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

// endpoint: /items/:item_id
func UpdateItem(ctx *gin.Context, dbase *sql.DB) {
	itemCtx, _ := ctx.Get("item")
	item, ok := itemCtx.(models.Item)
	if !ok {
		log.Println("Не удалось распарсить")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	log.Println(item)
	if err := db.UpdateItem(dbase, item); err != nil {
		log.Println("Ошибка обновления записи в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}
