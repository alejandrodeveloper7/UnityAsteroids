using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolsACG.ApiCaller.PokeApi
{
    public class PokeApiService : ApiCaller<PokeApiService>
    {
        #region Data
     
        public Pokemon PikachuData;
        public Pokemon CharmanderData;

        private readonly int _localHostPort = 0;

        #endregion

        #region Pikachu Data

        private readonly string _pikachuDataURL = "pokemon/Pikachu";
        public async Task<bool> GetPikachuData()
        {
            Pokemon response = await SendJsonRequest<Pokemon>(_pikachuDataURL, new RequestEmpty(), _localHostPort, HttpMethod.Get);

            if (response == null)
                return false;

            PikachuData = response;
            return true;
        }

        public async Task<bool> EnqueueGetPikachuData()
        {
            Pokemon response = await EnqueueJsonRequest<Pokemon>(_pikachuDataURL, new RequestEmpty(), _localHostPort, HttpMethod.Get);

            if (response == null)
                return false;

            PikachuData = response;
            return true;
        }

        public async Task<bool> GeneralEnqueueGetPikachuData()
        {
            Pokemon response = await GeneralQueueService.Instance.AddJsonRequest<Pokemon>(_pikachuDataURL, new RequestEmpty(), _localHostPort, HttpMethod.Get);

            if (response == null)
                return false;

            PikachuData = response;
            return true;
        }

        #endregion

        #region Charmander Data

        private readonly string _charmanderDataURL = "pokemon/Charmander";
        public async Task<bool> GetCharmanderData()
        {
            Pokemon response = await SendJsonRequest<Pokemon>(_charmanderDataURL, new RequestEmpty(), _localHostPort, HttpMethod.Get);

            if (response == null)
                return false;

            CharmanderData = response;
            return true;
        }

        public async Task<bool> EnqueueGetCharmanderData()
        {
            Pokemon response = await EnqueueJsonRequest<Pokemon>(_charmanderDataURL, new RequestEmpty(), _localHostPort, HttpMethod.Get);

            if (response == null)
                return false;

            CharmanderData = response;
            return true;
        }

        public async Task<bool> GeneralEnqueueGetCharmanderData()
        {
            Pokemon response = await GeneralQueueService.Instance.AddJsonRequest<Pokemon>(_charmanderDataURL, new RequestEmpty(), _localHostPort, HttpMethod.Get);

            if (response == null)
                return false;

            CharmanderData = response;
            return true;
        }

        #endregion

    }

    #region Pokemon Models

    [Serializable]
    public class Pokemon : ResponseEmpty
    {
        public int id;
        public string name;
        public int base_experience;
        public int height;
        public bool is_default;
        public int order;
        public int weight;
        public List<AbilityEntry> abilities;
        public List<NameUrl> forms;
        public List<GameIndex> game_indices;
        public List<HeldItem> held_items;
        public string location_area_encounters;
        public List<MoveEntry> moves;
        public NameUrl species;
        public Sprites sprites;
        public Cries cries;
        public List<StatEntry> stats;
        public List<TypeEntry> types;
        public List<PastType> past_types;
        public List<PastAbility> past_abilities;
    }

    [Serializable]
    public class AbilityEntry
    {
        public bool is_hidden;
        public int slot;
        public NameUrl ability;
    }

    [Serializable]
    public class NameUrl
    {
        public string name;
        public string url;
    }

    [Serializable]
    public class GameIndex
    {
        public int game_index;
        public NameUrl version;
    }

    [Serializable]
    public class HeldItem
    {
        public NameUrl item;
        public List<VersionDetail> version_details;
    }

    [Serializable]
    public class VersionDetail
    {
        public int rarity;
        public NameUrl version;
    }

    [Serializable]
    public class MoveEntry
    {
        public NameUrl move;
        public List<VersionGroupDetail> version_group_details;
    }

    [Serializable]
    public class VersionGroupDetail
    {
        public int level_learned_at;
        public NameUrl version_group;
        public NameUrl move_learn_method;
        public int? order;
    }

    [Serializable]
    public class Sprites
    {
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
        public OtherSprites other;
        public VersionSprites versions;
    }

    [Serializable]
    public class OtherSprites
    {
        public DreamWorld dream_world;
        public Home home;
        public OfficialArtwork official_artwork;
        public Showdown showdown;
    }

    [Serializable]
    public class DreamWorld
    {
        public string front_default;
        public string front_female;
    }

    [Serializable]
    public class Home
    {
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class OfficialArtwork
    {
        public string front_default;
        public string front_shiny;
    }

    [Serializable]
    public class Showdown
    {
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class VersionSprites
    {
        public GenerationI generation_i;
        public GenerationII generation_ii;
        public GenerationIII generation_iii;
        public GenerationIV generation_iv;
        public GenerationV generation_v;
        public GenerationVI generation_vi;
        public GenerationVII generation_vii;
        public GenerationVIII generation_viii;
    }

    [Serializable]
    public class GenerationI
    {
        public RedBlue red_blue;
        public Yellow yellow;
    }

    [Serializable]
    public class RedBlue
    {
        public string back_default;
        public string back_gray;
        public string front_default;
        public string front_gray;
    }

    [Serializable]
    public class Yellow
    {
        public string back_default;
        public string back_gray;
        public string front_default;
        public string front_gray;
    }

    [Serializable]
    public class GenerationII
    {
        public Crystal crystal;
        public Gold gold;
        public Silver silver;
    }

    [Serializable]
    public class Crystal
    {
        public string back_default;
        public string back_shiny;
        public string front_default;
        public string front_shiny;
    }

    [Serializable]
    public class Gold
    {
        public string back_default;
        public string back_shiny;
        public string front_default;
        public string front_shiny;
    }

    [Serializable]
    public class Silver
    {
        public string back_default;
        public string back_shiny;
        public string front_default;
        public string front_shiny;
    }

    [Serializable]
    public class GenerationIII
    {
        public Emerald emerald;
        public FireredLeafgreen firered_leafgreen;
        public RubySapphire ruby_sapphire;
    }

    [Serializable]
    public class Emerald
    {
        public string front_default;
        public string front_shiny;
    }

    [Serializable]
    public class FireredLeafgreen
    {
        public string back_default;
        public string back_shiny;
        public string front_default;
        public string front_shiny;
    }

    [Serializable]
    public class RubySapphire
    {
        public string back_default;
        public string back_shiny;
        public string front_default;
        public string front_shiny;
    }

    [Serializable]
    public class GenerationIV
    {
        public DiamondPearl diamond_pearl;
        public HeartgoldSoulsilver heartgold_soulsilver;
        public Platinum platinum;
    }

    [Serializable]
    public class DiamondPearl
    {
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class HeartgoldSoulsilver
    {
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class Platinum
    {
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class GenerationV
    {
        public BlackWhite black_white;
    }

    [Serializable]
    public class BlackWhite
    {
        public Animated animated;
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class Animated
    {
        public string back_default;
        public string back_female;
        public string back_shiny;
        public string back_shiny_female;
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class GenerationVI
    {
        public OmegarubyAlphasapphire omegaruby_alphasapphire;
        public XY x_y;
    }

    [Serializable]
    public class OmegarubyAlphasapphire
    {
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class XY
    {
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class GenerationVII
    {
        public Icons icons;
        public UltraSunUltraMoon ultra_sun_ultra_moon;
    }

    [Serializable]
    public class Icons
    {
        public string front_default;
        public string front_female;
    }

    [Serializable]
    public class UltraSunUltraMoon
    {
        public string front_default;
        public string front_female;
        public string front_shiny;
        public string front_shiny_female;
    }

    [Serializable]
    public class GenerationVIII
    {
        public Icons icons;
    }

    [Serializable]
    public class Cries
    {
        public string latest;
        public string legacy;
    }

    [Serializable]
    public class StatEntry
    {
        public int base_stat;
        public int effort;
        public NameUrl stat;
    }

    [Serializable]
    public class TypeEntry
    {
        public int slot;
        public NameUrl type;
    }

    [Serializable]
    public class PastType
    {
        public NameUrl generation;
        public List<TypeEntry> types;
    }

    [Serializable]
    public class PastAbility
    {
        public NameUrl generation;
        public List<AbilitySlot> abilities;
    }

    [Serializable]
    public class AbilitySlot
    {
        public NameUrl ability;
        public bool is_hidden;
        public int slot;
       }

        #endregion
    }
