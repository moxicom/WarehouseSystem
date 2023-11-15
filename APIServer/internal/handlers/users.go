package handlers

import (
	"APIServer/internal/db"
	"database/sql"
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
