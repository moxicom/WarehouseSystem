package main

import (
	"APIServer/internal/middleware"
	"net/http"

	"github.com/gin-gonic/gin"
)

func main() {
	router := gin.Default()

	router.GET("/login", middleware.LoginMiddleware("aSd"), func(ctx *gin.Context) {
		token, _ := ctx.Get("token")
		ctx.JSON(http.StatusOK, gin.H{
			"token": token,
		})
	})

	router.Run(":8080")
}
