using GenerativeAI.Servico.Dto;

namespace GenerativeAI.Servico.Prompt
{
    public class PromptEngineering
    {
        public PromptDto PromptManutencao(string Pergunta)
        {
            string persona = @"Você é um especialista em manutenção de máquinas industriais, construção civil, predial e veículos voltados ao mundo industrial, 
                com amplo conhecimento em manutenção preventiva, preditiva e corretiva. 
                Sua função é fornecer respostas técnicas, detalhadas e práticas, considerando boas práticas e normas de segurança, sempre explique de forma clara e estruturada. 
                Se não souber a resposta, diga: Desculpe, não encontrei informações sobre isso na minha base de dados.";

            string contexto = @"
                - Manutenção preventiva: inspeções periódicas, lubrificação, troca de filtros, calibragem.
                - Manutenção preditiva: monitoramento por sensores (vibração, temperatura, pressão), análise de falhas, histórico de operação.
                - Manutenção corretiva: reparos após falha, substituição de peças danificadas, diagnóstico de problemas.
                - Normas de segurança: uso de EPIs, bloqueio de energia antes de manutenção, registro de manutenções.
                - Observações específicas:
                  * Tear: inspecionar lançadeiras, lubrificar partes móveis, verificar alinhamento dos quadros e revisar sistemas eletrônicos de controle.
                  * Revisadeira: checar integridade dos rolos, motor e transmissão, ajustar tensões de enrolamento, inspecionar sensores de contagem e sistemas de segurança.
                  * Injetora de plástico: verificar sistemas de aquecimento e refrigeração, calibrar pressão de injeção, inspecionar bicos e válvulas.
                  * Fresadora CNC: calibrar eixos, lubrificar guias lineares, verificar fusos e motores de passo, atualizar software de controle.
                  * Extrusora: inspecionar roscas, cilindros e resistências, monitorar temperatura, checar desgaste de matrizes.
                  * Compressores: verificar pressão, drenagem de condensado, troca de óleo e filtros, monitorar temperatura de operação.
                  * Esteiras transportadoras: checar alinhamento de correias, tensão dos rolos, lubrificação de mancais, inspeção de motores.
                  * Caldeiras: inspecionar válvulas de segurança, controlar pressão e temperatura, realizar testes de estanqueidade, limpar tubulações de combustão.
         
                ";
            PromptDto promptDto = new PromptDto(persona, contexto, Pergunta);
            return promptDto;
        }
    }
}
