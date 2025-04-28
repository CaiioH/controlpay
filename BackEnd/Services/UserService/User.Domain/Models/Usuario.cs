using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Domain.Utilities;

namespace User.Domain.Models
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public NivelPermissao NivelPermissao { get; private set; }
        public bool Ativo { get; private set; } = true;

        public Usuario() { }

        public Usuario(string nome, string email, string senha, NivelPermissao nivelPermissao)
        {
            DefinirNome(nome);
            DefinirEmail(email);
            DefinirSenha(senha);
            DataCriacao = DateTime.UtcNow;
            NivelPermissao = nivelPermissao;
            Ativo = true;
        }

        private void DefinirNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio ou nulo.");

            Nome = nome;
        }
        private void DefinirEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                throw new ArgumentException("Email invalido.");

            Email = email.ToLower();
        }
        private void DefinirSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) || senha.Length < 6)
                throw new ArgumentException("Senha não pode ser vazia ou menor que 6 caracteres.");

            Console.WriteLine($"Antes de criptografar: {senha}");
            Senha = CriptografarSenha(senha);
            Console.WriteLine($"Depois de criptografar: {Senha}");
        }

        private string CriptografarSenha(string senha)
        {
            // Aqui você pode integrar depois com uma lib de hash
            Console.WriteLine($"criptografando senha: {senha}");
            var cript = BCrypt.Net.BCrypt.HashPassword(senha);
            Console.WriteLine($"senha criptografada: {cript}");
            return cript;
        }

        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;

        public void AlterarSenha(string novaSenha)
        {
            DefinirSenha(novaSenha);
        }

        public void AtualizarDados(string novoNome, string novoEmail)
        {
            DefinirNome(novoNome);
            DefinirEmail(novoEmail);
        }
    }
}