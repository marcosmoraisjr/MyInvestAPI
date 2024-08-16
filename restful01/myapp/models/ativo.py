# myapp/models/ativo.py
from django.db import models

class Ativo(models.Model):
    codigo = models.CharField(max_length=20, unique=True)
    descricao = models.CharField(max_length=255)
    tipo = models.CharField(max_length=50)
    data_criacao = models.DateTimeField(auto_now_add=True)
    data_update = models.DateTimeField(auto_now=True)

    class Meta:
        ordering = ('descricao',)
        
    def __str__(self):
        #return self.codigo
        return f'{self.codigo} - {self.descricao}'
