(function(window){'use strict';
    function SynthesizerLanguageConfiguration(){
            var SynthesizerLanguagePack = [
                {
                    id:1,
                    deutsch:"Zurücksetzen Synthesizer.net Konfiguration",
                    spanish:"Restablecer configuración de Synthesizer.net",
                    english:"Reset Synthesizer.net configuration"
                },
                {
                    id:2,
                    deutsch:"Synthesizer.net lautstärke",
                    spanish:"Volumen de Synthesizer.net",
                    english:"Synthesizer.net Volume"
                },
                {
                    id:3,
                    deutsch:"Synthesizer.net Konfiguration",
                    spanish:"Configuración de Synthesizer.net",
                    english:"Synthesizer.net Config"
                },
                {
                    id:4,
                    deutsch:"Synthetisieren Text",
                    spanish:"Sintetizar texto",
                    english:"Speak text"
                },
                {
                    id:5,
                    deutsch:"Text zu Sprache",
                    spanish:"Texto a hablar",
                    english:"Text to speech"
                },
                {
                    id:6,
                    deutsch:"Pause",
                    spanish:"Pausar",
                    english:"Pause"
                },
                {
                    id:7,
                    deutsch:"Aufhören",
                    spanish:"Detener",
                    english:"Stop"
                },
                {
                    id:8,
                    deutsch:"Export in Audio-Datei",
                    spanish:"Exportar a un archivo de audio",
                    english:"Export to audio file"
                }
            ];
        
        
            var synthesizerLangManager = {};
            
            synthesizerLangManager.setLanguage = function(lenguaje){
                $('[data-synthesizer-trans-id]').each(function(){
                    var id = $(this).data("synthesizer-trans-id");
                    var lang = synthesizerLangManager.getLanguageItemById(id);
                    
                    switch(lenguaje){
                        case "de-DE":
                            if($(this).attr("data-original-title")){
                                $(this).attr("data-original-title",lang.deutsch);
                            }else{
                                $(this).text(lang.deutsch);
                            }
                            break;
                        case "es-ES":
                            if($(this).data("data-original-title")){
                                $(this).attr("data-original-title",lang.spanish);
                            }else{
                                $(this).text(lang.spanish);
                            }
                           break;
                        case "en-EN":
                           if($(this).data("data-original-title")){
                                $(this).attr("original-title",lang.english);
                            }else{
                                $(this).text(lang.english);
                            }
                           break;
                        default:
                           $(this).text(lang.english);
                    }
                });
            };
            
            synthesizerLangManager.getLanguageItemById = function(id){
                var result = $.grep(SynthesizerLanguagePack, function(e){ return e.id == id; });
                
                if (result.length == 0) {
                    return null;
                } else if (result.length == 1) {
                    return result[0];
                } else {
                    throw new error("The SynthesizerLanguagePack have found more than one item with the same id identificator !");
                }
            };
            
            synthesizerLangManager.getConfiguration = function(callback){
                $.getJSON("../../dist/config/ideconfig.json", function(json) {
                    callback(json);
                });
            };
            
        return synthesizerLangManager;
    }
    
    if(typeof(synthesizerLangManager) === 'undefined'){
        window.synthesizerLangManager = SynthesizerLanguageConfiguration();
    }
})(window);
