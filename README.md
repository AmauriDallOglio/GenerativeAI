# GenerativeAI

O projeto GenerativeAI é responsável por centralizar e orquestrar a comunicação entre os serviços internos da solução: RAG, ML.NET, Youtube e Whisper. O projeto implementa um agente orquestrador baseado em APIs, onde cada serviço integrado atua como uma ferramenta especializada.

Ele funciona como uma camada principal de integração. Em vez de o consumidor chamar cada serviço separadamente, o GenerativeAI expõe rotas próprias e encaminha as requisições para os serviços corretos, já aplicando configurações, API Keys e tratamento padronizado de respostas.


Responsabilidades
- Autenticação dos usuários (JWT)
- Exposição das rotas principais da aplicação.
- Chamada dos serviços internos
- Orquestração entre RAG, ML.NET, Youtube e Whisper.
- Centralização das configurações de URL e API Key dos serviços.
- Envio automático da API Key no header X-Api-Key.
- Tratamento das respostas usando ResultadoOperacao
- Propagação de CancellationToken em operações demoradas.

<img width="1897" height="1016" alt="image" src="https://github.com/user-attachments/assets/f3b3649d-61dc-4bd8-8092-907509629aa8" />

Serviços Integrados
- RAG
  - Armazena documentos e textos.
  - Permite importar arquivos.
  - Permite importar texto diretamente.
  - Permite consultar os conteúdos já processados.
- ML.NET
  - Gera treinamento.
  - Consulta treinamento existente.
  - Atualiza treinamento.
  - Obtém respostas com base no modelo treinado.
  - Lista sessões.
- Youtube
  - Baixa o áudio de vídeos do Youtube.
  - Retorna informações do arquivo gerado, como nome e caminho do áudio.
- Whisper
  - Recebe arquivos de áudio.
  - Transcreve áudio para texto.
  - Retorna a transcrição para uso por outros serviços.

 <img width="1821" height="553" alt="image" src="https://github.com/user-attachments/assets/01bd2c43-919b-414a-bbeb-30238399f965" />

- /api/GenerativeAi/TreinamentoYoutube
  - Envia uma URL de vídeo para o GenerativeAI.
  - O GenerativeAI chama o serviço Youtube para baixar o áudio.
  - Após o download terminar, o arquivo de áudio é enviado ao Whisper.
  - O Whisper transcreve o áudio para texto.
  - O texto transcrito é enviado ao RAG.
  - O RAG importa o texto e gera a estrutura de busca.
  - O GenerativeAI retorna o resultado final usando o padrão ResultadoOperacao.
 



 



