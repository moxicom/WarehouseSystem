package handlers

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

func AuthHandler(ctx *gin.Context) {
	user, _ := ctx.Get("user")
	ctx.JSON(http.StatusOK, user)
}
