1) Adicionar a BD ao projeto:
No Visual Studio, clicar com o botão direito na pasta do projeto no Gerenciador de Soluções.
Selecionar Adicionar -> Item existente...
Navegar até à localização da base de dados (.mdf) e adicioná-la ao projeto.

2) Definir as propriedades do ficheiro da BD:
Após adicionar a base de dados ao projeto, clicar com o botão direito no arquivo da base de dados na Gerenciador de Soluções e selecionar Propriedades.
Na janela de propriedades, localizar a propriedade Copiar para Diretório de Saída.
Definir esta propriedade como Copiar se for mais novo ou Copiar sempre. Isto garantirá que a BD seja copiada para a pasta bin\Debug sempre que compilarmos o projeto.

3) Modificar a String de Conexão:
Alterar a string de conexão no código de forma a apontar para a localização correta da base de dados na pasta bin\Debug. 
Exemplo:
string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\\ClubeBD.mdf;Integrated Security=True;Connect Timeout=30";

4)Testar a aplicação:
Executar a aplicação em modo de depuração (Debug) e verificar se a base de dados está sendo acedida corretamente.
Devemos também verificar se a base de dados foi copiada para a pasta bin\Debug após a construção do projeto.



