# myapp/models/carteira.py
from django.db import models
from .ativo import Ativo
from .usuario import Usuario

class Carteira(models.Model):
    #usuario = models.ForeignKey(Usuario, on_delete=models.CASCADE)
    ativo = models.ForeignKey(Ativo, on_delete=models.CASCADE)
    descricao = models.CharField(max_length=255)
    data_criacao = models.DateTimeField(auto_now_add=True)
    data_update = models.DateTimeField(auto_now=True)

    def __str__(self):
        return f"{self.descricao} - {self.usuario.username}"
