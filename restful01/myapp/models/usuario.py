# myapp/models/usuario.py
from django.db import models
from django.contrib.auth.models import AbstractUser, Group, Permission
#AbstractUser: Ao herdar de AbstractUser, você está usando o modelo padrão de usuário do Django e estendendo-o com campos adicionais, como telefone, data_criacao e data_update.

class Usuario(AbstractUser):
    telefone = models.CharField(max_length=20, null=True, blank=True)
    data_criacao = models.DateTimeField(auto_now_add=True)
    data_update = models.DateTimeField(auto_now=True)

    # Define os relacionamentos com os grupos e permissões
    groups = models.ManyToManyField(
        Group,
        related_name='usuario_set',  # Nome único para o relacionamento reverso
        blank=True,
    )
    user_permissions = models.ManyToManyField(
        Permission,
        related_name='usuario_set',  # Nome único para o relacionamento reverso
        blank=True,
    )
