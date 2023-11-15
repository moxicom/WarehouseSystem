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

func GetAllUsers(ctx *gin.Context, dbase *sql.DB) {
	users, err := db.GetAllUsers(dbase)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, users)
}

// endpoint: /users
func InsertUser(ctx *gin.Context, dbase *sql.DB) {
	userCtx, _ := ctx.Get("data")
	user, ok := userCtx.(models.User)
	if !ok {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	log.Println(user)
	exist, err := db.IsUserExist(dbase, user.Username)
	if err != nil {
		log.Println("Ошибка поиска проверки существования пользователя в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	if exist {
		ctx.JSON(http.StatusConflict, gin.H{"error": "User already exists"})
		return
	}
	if err := db.InsertUser(dbase, user); err != nil {
		log.Println("Ошибка обновления записи в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

// endpoint: /users/:user_id
func DeleteUserByID(ctx *gin.Context, dbase *sql.DB) {
	userIDstr := ctx.Param("user_id")
	userIDInt, err := strconv.Atoi(userIDstr)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	err = db.DeleteUserByID(dbase, userIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"message": "User deleted successfully"})
}
