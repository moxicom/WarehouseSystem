package handlers

import (
	"APIServer/internal/models"
	"log"
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
)

func (h *handler) getAllUsers(ctx *gin.Context) {
	users, err := h.r.GetAllUsers()
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, users)
}

// endpoint: /users
func (h *handler) insertUser(ctx *gin.Context) {
	userCtx, _ := ctx.Get("data")
	user, ok := userCtx.(models.User)
	if !ok {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	exist, err := h.r.IsUserExist(user.Username)
	if err != nil {
		log.Println("Ошибка поиска проверки существования пользователя в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	if exist {
		ctx.JSON(http.StatusConflict, gin.H{"error": "User already exists"})
		return
	}
	if err := h.r.InsertUser(user); err != nil {
		log.Println("Ошибка обновления записи в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}

// endpoint: /users/:user_id
func (h *handler) deleteUserByID(ctx *gin.Context) {
	userIDstr := ctx.Param("user_id")
	userIDInt, err := strconv.Atoi(userIDstr)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	err = h.r.DeleteUserByID(userIDInt)
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"message": "User deleted successfully"})
}

// endpoint: /users/user_id
func (h *handler) updateUser(ctx *gin.Context) {
	userCtx, _ := ctx.Get("data")
	user, ok := userCtx.(models.User)
	if !ok {
		log.Println("Не удалось распарсить")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	if err := h.r.UpdateUser(user); err != nil {
		log.Println("Ошибка обновления записи в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}
