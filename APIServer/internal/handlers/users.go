package handlers

import (
	"APIServer/internal/db"
	"database/sql"
	"net/http"

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
