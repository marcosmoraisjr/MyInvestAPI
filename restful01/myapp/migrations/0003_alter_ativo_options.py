# Generated by Django 5.1 on 2024-08-16 13:25

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('myapp', '0002_rename_carteiraativos_carteira'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='ativo',
            options={'ordering': ('descricao',)},
        ),
    ]