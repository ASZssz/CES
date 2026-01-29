using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Celeste.Core;

namespace Celeste.Core.Data
{
    /// <summary>
    /// Sistema de persistência de dados para Celeste
    /// Suporta Desktop e Android via IPlatformService
    /// </summary>
    public class SaveDataManager
    {
        private readonly IPlatformService _platform;
        private readonly ILogSystem _logger;
        private readonly string _savePath;
        private const string SAVE_FOLDER = "SaveData";
        private const string PROFILE_FILE = "profile.json";

        public SaveDataManager(IPlatformService platform, ILogSystem logger)
        {
            _platform = platform;
            _logger = logger;
            _savePath = Path.Combine(platform.GetAppDataPath(), SAVE_FOLDER);

            EnsureSaveDirectoryExists();
        }

        private void EnsureSaveDirectoryExists()
        {
            try
            {
                if (!Directory.Exists(_savePath))
                {
                    Directory.CreateDirectory(_savePath);
                    _logger.Log($"[SaveData] Criado diretório: {_savePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"[SaveData] ERRO ao criar diretório: {ex.Message}");
            }
        }

        public async Task<PlayerProfile> LoadProfileAsync()
        {
            try
            {
                string profilePath = Path.Combine(_savePath, PROFILE_FILE);

                if (!File.Exists(profilePath))
                {
                    _logger.Log("[SaveData] Nenhum perfil encontrado, criando novo");
                    return new PlayerProfile { TimeCreated = DateTime.Now };
                }

                string json = await File.ReadAllTextAsync(profilePath);
                var profile = JsonSerializer.Deserialize<PlayerProfile>(json);
                _logger.Log($"[SaveData] Perfil carregado: {profile?.PlayerName}");
                return profile ?? new PlayerProfile { TimeCreated = DateTime.Now };
            }
            catch (Exception ex)
            {
                _logger.Log($"[SaveData] ERRO ao carregar perfil: {ex.Message}");
                return new PlayerProfile { TimeCreated = DateTime.Now };
            }
        }

        public async Task SaveProfileAsync(PlayerProfile profile)
        {
            try
            {
                string profilePath = Path.Combine(_savePath, PROFILE_FILE);
                string json = JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(profilePath, json);
                _logger.Log($"[SaveData] Perfil salvo: {profile.PlayerName}");
            }
            catch (Exception ex)
            {
                _logger.Log($"[SaveData] ERRO ao salvar perfil: {ex.Message}");
            }
        }

        public async Task<Dictionary<string, object>> LoadGameStateAsync(string levelName)
        {
            try
            {
                string statePath = Path.Combine(_savePath, $"{levelName}.json");

                if (!File.Exists(statePath))
                {
                    return new Dictionary<string, object>();
                }

                string json = await File.ReadAllTextAsync(statePath);
                var state = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                _logger.Log($"[SaveData] Estado do nível carregado: {levelName}");
                return state ?? new Dictionary<string, object>();
            }
            catch (Exception ex)
            {
                _logger.Log($"[SaveData] ERRO ao carregar estado: {ex.Message}");
                return new Dictionary<string, object>();
            }
        }

        public async Task SaveGameStateAsync(string levelName, Dictionary<string, object> state)
        {
            try
            {
                string statePath = Path.Combine(_savePath, $"{levelName}.json");
                string json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(statePath, json);
                _logger.Log($"[SaveData] Estado do nível salvo: {levelName}");
            }
            catch (Exception ex)
            {
                _logger.Log($"[SaveData] ERRO ao salvar estado: {ex.Message}");
            }
        }

        public void DeleteSaveData()
        {
            try
            {
                if (Directory.Exists(_savePath))
                {
                    Directory.Delete(_savePath, true);
                    _logger.Log("[SaveData] Todos os dados foram deletados");
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"[SaveData] ERRO ao deletar dados: {ex.Message}");
            }
        }
    }

    public class PlayerProfile
    {
        public string PlayerName { get; set; } = "Madeline";
        public int CompletedLevels { get; set; } = 0;
        public int TotalDeaths { get; set; } = 0;
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public DateTime LastPlayed { get; set; } = DateTime.Now;
        public Dictionary<string, LevelStats> LevelProgress { get; set; } = new();
    }

    public class LevelStats
    {
        public string LevelName { get; set; }
        public bool Completed { get; set; }
        public int Deaths { get; set; }
        public int DeathsSession { get; set; }
        public TimeSpan BestTime { get; set; }
        public int Strawberries { get; set; }
        public DateTime FirstPlayedTime { get; set; }
    }
}
