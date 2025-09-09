# Sistema de Pedidos Simples:
Projeto com três funcionalidades principais: gestão de pedidos, gestão de produtos e gestão de clientes;

## Setup Inicial:
O projeto usa .Net 8 e um banco de dados SQL, além disso usa para a conexão com o banco de dados o dapper e para testes unitários XUnit e Moq, sendo assim, são pacotes necessários de serem instalados

## Estrutura do projeto:
O projeto foi feito usando uma estrutura em camadas dividida em Domain: onde ficam as representações das entidades; Application: onde ficam os serviços e poderiam vir a receber regras de negócio, Infrastructure: onde é realizada a integração com o banco via Dapper e Presentation a qual o usuário acessa diretamente;

## Funcionalidades:
É possível criar, editar e deletar clientes e produtos;
Também é possível criar pedidos, alocando um cliente e multiplos produtos, não é possível deletar os pedidos, mas é possível alterar o status do produto. Quando o status do produto é alterado, é criado um novo registro na tabela notificacoes

## Testes unitários:
Existem alguns testes simples feitos para a camada Application para os serviços de ItemPedidos e Clientes;

## Banco de dados: 
Dentro da pasta Util da camada de Infra há um script SQL para criação do banco e inserção de alguns dados iniciais;

## Layout:
O Layout do sistema foi feito usando bootstrap, select2 e sweetalert;
