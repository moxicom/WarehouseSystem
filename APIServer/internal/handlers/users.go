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

type userData struct {
	UserID int         `json:"userID"`
	Data   models.User `json:"data"`
}

// endpoint: /users
func (h *handler) insertUser(ctx *gin.Context) {
	var input userData
	if err := ctx.ShouldBindJSON(&input); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	log.Println(input)
	exist, err := h.r.IsUserExist(input.Data.Username)
	if err != nil {
		log.Println("Ошибка поиска проверки существования пользователя в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	if exist {
		ctx.JSON(http.StatusConflict, gin.H{"error": "User already exists"})
		return
	}
	if err := h.r.InsertUser(input.Data); err != nil {
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
	var input userData
	if err := ctx.ShouldBindJSON(&input); err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": "Неверный формат данных"})
		return
	}
	userID, err := strconv.Atoi(ctx.Param("user_id"))
	if err != nil {
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	input.Data.ID = userID
	log.Println(input)
	if err := h.r.UpdateUser(input.Data); err != nil {
		log.Println("Ошибка обновления записи в базе данных")
		ctx.JSON(http.StatusInternalServerError, gin.H{"error": err.Error()})
		return
	}
	ctx.JSON(http.StatusOK, gin.H{"status": "ok"})
}
