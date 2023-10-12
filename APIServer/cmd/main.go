package main

import (
	"net/http"

	"github.com/gin-gonic/gin"
)

func main() {
	router := gin.Default()

	router.GET("/login", func(ctx *gin.Context) {
		ctx.String(http.StatusOK, "ПРИВЕТ ИЗ GO")
	})

	router.Run(":8080")
}
