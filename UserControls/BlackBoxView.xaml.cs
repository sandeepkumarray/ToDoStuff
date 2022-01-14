using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoStuff.Model;
using Attribute = ToDoStuff.Model.Attribute;

namespace ToDoStuff.UserControls
{
    /// <summary>
    /// Interaction logic for BlackBoxView.xaml
    /// </summary>
    [ControlNameAttribute("BlackBox")]
    public partial class BlackBoxView : UserControl, IControlsInterface
    {
        public BlackBoxView()
        {
            InitializeComponent();

            LoadTestData();
        }

        private void LoadTestData()
        {
            txtFilePath.Text = @"C:\Users\sande\Desktop\ScrapOut\manytoon.txt";
            txtPostfix.Text = "/";
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string basePath = txtBasePath.Text.Trim();
            string filePath = txtFilePath.Text.Trim();
            int from = Convert.ToInt32(txtFrom.Text.Trim());
            int to = Convert.ToInt32(txtTo.Text.Trim());
            string postfix = txtPostfix.Text.Trim();

            StringBuilder sb = new StringBuilder();

            while (from <= to)
            {
                sb.AppendLine(basePath + from++ + postfix);
            }

            FileUtility.SaveDataToFile(filePath, sb.ToString(), false);

            MessageBox.Show("Complete");
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //GenerateCodeFromJSON();
            CreateContentCategoriesJSON();
            MessageBox.Show("Done");
        }

        void CreateContentCategoriesJSON()
        {
            ContentTemplate contentTemplate = new ContentTemplate();
            contentTemplate.TemplateName = "Content Template";
            contentTemplate.Contents = new List<Content>();

            #region CODE
            Content content_buildings = new Content("buildings", true);

            Category buildings_amenities = new Category(8, "Amenities", "amenities", "amenities");



            Category buildings_design = new Category(3, "Design", "design", "design");

            Attribute buildings_design_Facade = new Attribute("Facade", "Facade", "tinytext", false, true);
            Attribute buildings_design_Floor_count = new Attribute("Floor_count", "Floor Count", "int", false, true);
            Attribute buildings_design_Dimensions = new Attribute("Dimensions", "Dimensions", "int", false, true);
            Attribute buildings_design_Architectural_style = new Attribute("Architectural_style", "Architectural Style", "tinytext", false, true);
            buildings_design.Attributes.Add(buildings_design_Facade);
            buildings_design.Attributes.Add(buildings_design_Floor_count);
            buildings_design.Attributes.Add(buildings_design_Dimensions);
            buildings_design.Attributes.Add(buildings_design_Architectural_style);


            Category buildings_financial = new Category(7, "Financial", "financial", "financial");

            Attribute buildings_financial_Price = new Attribute("Price", "Price", "double", false, true);
            buildings_financial.Attributes.Add(buildings_financial_Price);


            Category buildings_gallery = new Category(10, "Gallery", "gallery", "gallery");



            Category buildings_history = new Category(9, "History", "history", "history");

            Attribute buildings_history_Architect = new Attribute("Architect", "Architect", "tinytext", false, true);
            Attribute buildings_history_Developer = new Attribute("Developer", "Developer", "tinytext", false, true);
            Attribute buildings_history_Notable_events = new Attribute("Notable_events", "Notable Events", "longtext", false, true);
            Attribute buildings_history_Constructed_year = new Attribute("Constructed_year", "Constructed Year", "int", false, true);
            Attribute buildings_history_Construction_cost = new Attribute("Construction_cost", "Construction Cost", "double", false, true);
            buildings_history.Attributes.Add(buildings_history_Architect);
            buildings_history.Attributes.Add(buildings_history_Developer);
            buildings_history.Attributes.Add(buildings_history_Notable_events);
            buildings_history.Attributes.Add(buildings_history_Constructed_year);
            buildings_history.Attributes.Add(buildings_history_Construction_cost);


            Category buildings_location = new Category(5, "Location", "location", "location");

            Attribute buildings_location_Address = new Attribute("Address", "Address", "tinytext", false, true);
            buildings_location.Attributes.Add(buildings_location_Address);


            Category buildings_neighborhood = new Category(6, "Neighborhood", "neighborhood", "neighborhood");

            Attribute buildings_neighborhood_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            buildings_neighborhood.Attributes.Add(buildings_neighborhood_Private_Notes);


            Category buildings_notes = new Category(11, "Notes", "notes", "notes");

            Attribute buildings_notes_Notes = new Attribute("Notes", "Notes", "longtext", false, true);
            Attribute buildings_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            buildings_notes.Attributes.Add(buildings_notes_Notes);
            buildings_notes.Attributes.Add(buildings_notes_Private_Notes);


            Category buildings_occupants = new Category(2, "Occupants", "occupants", "occupants");

            Attribute buildings_occupants_Owner = new Attribute("Owner", "Owner", "tinytext", false, true);
            Attribute buildings_occupants_Tenants = new Attribute("Tenants", "Tenants", "tinytext", false, true);
            Attribute buildings_occupants_Affiliation = new Attribute("Affiliation", "Affiliation", "tinytext", false, true);
            Attribute buildings_occupants_Capacity = new Attribute("Capacity", "Capacity", "int", false, true);
            buildings_occupants.Attributes.Add(buildings_occupants_Owner);
            buildings_occupants.Attributes.Add(buildings_occupants_Tenants);
            buildings_occupants.Attributes.Add(buildings_occupants_Affiliation);
            buildings_occupants.Attributes.Add(buildings_occupants_Capacity);


            Category buildings_overview = new Category(1, "Overview", "overview", "overview");

            Attribute buildings_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute buildings_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute buildings_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute buildings_overview_Type_of_building = new Attribute("Type_of_building", "Type Of Building", "tinytext", false, true);
            Attribute buildings_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "tinytext", false, true);
            Attribute buildings_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            buildings_overview.Attributes.Add(buildings_overview_Name);
            buildings_overview.Attributes.Add(buildings_overview_Universe);
            buildings_overview.Attributes.Add(buildings_overview_Description);
            buildings_overview.Attributes.Add(buildings_overview_Type_of_building);
            buildings_overview.Attributes.Add(buildings_overview_Alternate_names);
            buildings_overview.Attributes.Add(buildings_overview_Tags);


            Category buildings_purpose = new Category(4, "Purpose", "purpose", "purpose");

            Attribute buildings_purpose_Permits = new Attribute("Permits", "Permits", "tinytext", false, true);
            Attribute buildings_purpose_Purpose = new Attribute("Purpose", "Purpose", "tinytext", false, true);
            buildings_purpose.Attributes.Add(buildings_purpose_Permits);
            buildings_purpose.Attributes.Add(buildings_purpose_Purpose);


            content_buildings.categories.Add(buildings_amenities);
            content_buildings.categories.Add(buildings_design);
            content_buildings.categories.Add(buildings_financial);
            content_buildings.categories.Add(buildings_gallery);
            content_buildings.categories.Add(buildings_history);
            content_buildings.categories.Add(buildings_location);
            content_buildings.categories.Add(buildings_neighborhood);
            content_buildings.categories.Add(buildings_notes);
            content_buildings.categories.Add(buildings_occupants);
            content_buildings.categories.Add(buildings_overview);
            content_buildings.categories.Add(buildings_purpose);


            Content content_characters = new Content("characters", true);

            Category characters_family = new Category(6, "Family", "family", "family");

            Attribute characters_family_pets = new Attribute("pets", "Pets", "text", false, true);
            characters_family.Attributes.Add(characters_family_pets);


            Category characters_gallery = new Category(8, "Gallery", "gallery", "gallery");



            Category characters_history = new Category(5, "History", "history", "history");

            Attribute characters_history_birthday = new Attribute("birthday", "Birthday", "text", false, true);
            Attribute characters_history_birthplace = new Attribute("birthplace", "Birthplace", "text", false, true);
            Attribute characters_history_education = new Attribute("education", "Education", "text", false, true);
            Attribute characters_history_background = new Attribute("background", "Background", "text", false, true);
            characters_history.Attributes.Add(characters_history_birthday);
            characters_history.Attributes.Add(characters_history_birthplace);
            characters_history.Attributes.Add(characters_history_education);
            characters_history.Attributes.Add(characters_history_background);


            Category characters_inventory = new Category(7, "Inventory", "inventory", "inventory");



            Category characters_looks = new Category(2, "Looks", "looks", "looks");

            Attribute characters_looks_height = new Attribute("height", "Height", "varchar", false, true);
            Attribute characters_looks_weight = new Attribute("weight", "Weight", "varchar", false, true);
            Attribute characters_looks_haircolor = new Attribute("haircolor", "Hair Color", "varchar", false, true);
            Attribute characters_looks_hairstyle = new Attribute("hairstyle", "Hair Style", "varchar", false, true);
            Attribute characters_looks_facialhair = new Attribute("facialhair", "Facial Hair", "varchar", false, true);
            Attribute characters_looks_eyecolor = new Attribute("eyecolor", "Eye Color", "varchar", false, true);
            Attribute characters_looks_race = new Attribute("race", "Race", "varchar", false, true);
            Attribute characters_looks_skintone = new Attribute("skintone", "Skin Tone", "varchar", false, true);
            Attribute characters_looks_bodytype = new Attribute("bodytype", "Body Type", "varchar", false, true);
            Attribute characters_looks_identmarks = new Attribute("identmarks", "Identifying Marks", "text", false, true);
            characters_looks.Attributes.Add(characters_looks_height);
            characters_looks.Attributes.Add(characters_looks_weight);
            characters_looks.Attributes.Add(characters_looks_haircolor);
            characters_looks.Attributes.Add(characters_looks_hairstyle);
            characters_looks.Attributes.Add(characters_looks_facialhair);
            characters_looks.Attributes.Add(characters_looks_eyecolor);
            characters_looks.Attributes.Add(characters_looks_race);
            characters_looks.Attributes.Add(characters_looks_skintone);
            characters_looks.Attributes.Add(characters_looks_bodytype);
            characters_looks.Attributes.Add(characters_looks_identmarks);


            Category characters_nature = new Category(3, "Nature", "nature", "nature");

            Attribute characters_nature_mannerisms = new Attribute("mannerisms", "Mannerisms", "text", false, true);
            Attribute characters_nature_motivations = new Attribute("motivations", "Motivations", "text", false, true);
            Attribute characters_nature_flaws = new Attribute("flaws", "Flaws", "text", false, true);
            Attribute characters_nature_talents = new Attribute("talents", "Talents", "text", false, true);
            Attribute characters_nature_hobbies = new Attribute("hobbies", "Hobbies", "text", false, true);
            Attribute characters_nature_personality_type = new Attribute("personality_type", "Personality Type", "text", false, true);
            characters_nature.Attributes.Add(characters_nature_mannerisms);
            characters_nature.Attributes.Add(characters_nature_motivations);
            characters_nature.Attributes.Add(characters_nature_flaws);
            characters_nature.Attributes.Add(characters_nature_talents);
            characters_nature.Attributes.Add(characters_nature_hobbies);
            characters_nature.Attributes.Add(characters_nature_personality_type);


            Category characters_notes = new Category(9, "Notes", "notes", "notes");

            Attribute characters_notes_notes = new Attribute("notes", "Notes", "text", false, true);
            Attribute characters_notes_private_notes = new Attribute("private_notes", "Private Notes", "text", false, true);
            characters_notes.Attributes.Add(characters_notes_notes);
            characters_notes.Attributes.Add(characters_notes_private_notes);


            Category characters_overview = new Category(1, "Overview", "overview", "overview");

            Attribute characters_overview_name = new Attribute("name", "Name", "varchar", false, true);
            Attribute characters_overview_role = new Attribute("role", "Role", "varchar", false, true);
            Attribute characters_overview_gender = new Attribute("gender", "Gender", "varchar", false, true);
            Attribute characters_overview_age = new Attribute("age", "Age", "varchar", false, true);
            Attribute characters_overview_universe_id = new Attribute("universe_id", "Universe", "int", false, true);
            Attribute characters_overview_aliases = new Attribute("aliases", "Aliases", "text", false, true);
            Attribute characters_overview_favorite = new Attribute("favorite", "Favorite", "tinyint", false, true);
            characters_overview.Attributes.Add(characters_overview_name);
            characters_overview.Attributes.Add(characters_overview_role);
            characters_overview.Attributes.Add(characters_overview_gender);
            characters_overview.Attributes.Add(characters_overview_age);
            characters_overview.Attributes.Add(characters_overview_universe_id);
            characters_overview.Attributes.Add(characters_overview_aliases);
            characters_overview.Attributes.Add(characters_overview_favorite);


            Category characters_social = new Category(4, "Social", "social", "social");

            Attribute characters_social_religion = new Attribute("religion", "Religion", "text", false, true);
            Attribute characters_social_politics = new Attribute("politics", "Politics", "text", false, true);
            Attribute characters_social_prejudices = new Attribute("prejudices", "Prejudices", "text", false, true);
            Attribute characters_social_occupation = new Attribute("occupation", "Occupation", "text", false, true);
            Attribute characters_social_fave_color = new Attribute("fave_color", "Fave Color", "varchar", false, true);
            Attribute characters_social_fave_food = new Attribute("fave_food", "Fave Food", "varchar", false, true);
            Attribute characters_social_fave_possession = new Attribute("fave_possession", "Fave Possession", "varchar", false, true);
            Attribute characters_social_fave_weapon = new Attribute("fave_weapon", "Fave Weapon", "varchar", false, true);
            Attribute characters_social_fave_animal = new Attribute("fave_animal", "Fave Animal", "varchar", false, true);
            characters_social.Attributes.Add(characters_social_religion);
            characters_social.Attributes.Add(characters_social_politics);
            characters_social.Attributes.Add(characters_social_prejudices);
            characters_social.Attributes.Add(characters_social_occupation);
            characters_social.Attributes.Add(characters_social_fave_color);
            characters_social.Attributes.Add(characters_social_fave_food);
            characters_social.Attributes.Add(characters_social_fave_possession);
            characters_social.Attributes.Add(characters_social_fave_weapon);
            characters_social.Attributes.Add(characters_social_fave_animal);


            content_characters.categories.Add(characters_family);
            content_characters.categories.Add(characters_gallery);
            content_characters.categories.Add(characters_history);
            content_characters.categories.Add(characters_inventory);
            content_characters.categories.Add(characters_looks);
            content_characters.categories.Add(characters_nature);
            content_characters.categories.Add(characters_notes);
            content_characters.categories.Add(characters_overview);
            content_characters.categories.Add(characters_social);


            Content content_conditions = new Content("conditions", true);

            Category conditions_analysis = new Category(5, "Analysis", "analysis", "analysis");

            Attribute conditions_analysis_Specialty_Field = new Attribute("Specialty_Field", "Specialty", "varchar", false, true);
            Attribute conditions_analysis_Rarity = new Attribute("Rarity", "Rarity", "varchar", false, true);
            Attribute conditions_analysis_Symbolism = new Attribute("Symbolism", "Symbolism", "varchar", false, true);
            conditions_analysis.Attributes.Add(conditions_analysis_Specialty_Field);
            conditions_analysis.Attributes.Add(conditions_analysis_Rarity);
            conditions_analysis.Attributes.Add(conditions_analysis_Symbolism);


            Category conditions_causes = new Category(2, "Causes", "causes", "causes");

            Attribute conditions_causes_Genetic_factors = new Attribute("Genetic_factors", "Genetic Factors", "varchar", false, true);
            Attribute conditions_causes_Environmental_factors = new Attribute("Environmental_factors", "Environmental Factors", "varchar", false, true);
            Attribute conditions_causes_Lifestyle_factors = new Attribute("Lifestyle_factors", "Lifestyle Factors", "varchar", false, true);
            Attribute conditions_causes_Transmission = new Attribute("Transmission", "Transmission", "varchar", false, true);
            Attribute conditions_causes_Epidemiology = new Attribute("Epidemiology", "Epidemiology", "varchar", false, true);
            conditions_causes.Attributes.Add(conditions_causes_Genetic_factors);
            conditions_causes.Attributes.Add(conditions_causes_Environmental_factors);
            conditions_causes.Attributes.Add(conditions_causes_Lifestyle_factors);
            conditions_causes.Attributes.Add(conditions_causes_Transmission);
            conditions_causes.Attributes.Add(conditions_causes_Epidemiology);


            Category conditions_effects = new Category(3, "Effects", "effects", "effects");

            Attribute conditions_effects_Visual_effects = new Attribute("Visual_effects", "Visual Effects", "varchar", false, true);
            Attribute conditions_effects_Mental_effects = new Attribute("Mental_effects", "Mental Effects", "varchar", false, true);
            Attribute conditions_effects_Symptoms = new Attribute("Symptoms", "Symptoms", "varchar", false, true);
            Attribute conditions_effects_Duration = new Attribute("Duration", "Duration", "varchar", false, true);
            Attribute conditions_effects_Prognosis = new Attribute("Prognosis", "Prognosis", "varchar", false, true);
            Attribute conditions_effects_Variations = new Attribute("Variations", "Variations", "varchar", false, true);
            conditions_effects.Attributes.Add(conditions_effects_Visual_effects);
            conditions_effects.Attributes.Add(conditions_effects_Mental_effects);
            conditions_effects.Attributes.Add(conditions_effects_Symptoms);
            conditions_effects.Attributes.Add(conditions_effects_Duration);
            conditions_effects.Attributes.Add(conditions_effects_Prognosis);
            conditions_effects.Attributes.Add(conditions_effects_Variations);


            Category conditions_gallery = new Category(7, "Gallery", "gallery", "gallery");



            Category conditions_history = new Category(6, "History", "history", "history");

            Attribute conditions_history_Origin = new Attribute("Origin", "Origin", "text", false, true);
            Attribute conditions_history_Evolution = new Attribute("Evolution", "Evolution", "varchar", false, true);
            conditions_history.Attributes.Add(conditions_history_Origin);
            conditions_history.Attributes.Add(conditions_history_Evolution);


            Category conditions_notes = new Category(8, "Notes", "notes", "notes");

            Attribute conditions_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute conditions_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            conditions_notes.Attributes.Add(conditions_notes_Notes);
            conditions_notes.Attributes.Add(conditions_notes_Private_Notes);


            Category conditions_overview = new Category(1, "Overview", "overview", "overview");

            Attribute conditions_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute conditions_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute conditions_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute conditions_overview_Type_of_condition = new Attribute("Type_of_condition", "Type Of Condition", "varchar", false, true);
            Attribute conditions_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "varchar", false, true);
            Attribute conditions_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            conditions_overview.Attributes.Add(conditions_overview_Name);
            conditions_overview.Attributes.Add(conditions_overview_Universe);
            conditions_overview.Attributes.Add(conditions_overview_Description);
            conditions_overview.Attributes.Add(conditions_overview_Type_of_condition);
            conditions_overview.Attributes.Add(conditions_overview_Alternate_names);
            conditions_overview.Attributes.Add(conditions_overview_Tags);


            Category conditions_treatment = new Category(4, "Treatment", "treatment", "treatment");

            Attribute conditions_treatment_Prevention = new Attribute("Prevention", "Prevention", "varchar", false, true);
            Attribute conditions_treatment_Diagnostic_method = new Attribute("Diagnostic_method", "Diagnostic Method", "varchar", false, true);
            Attribute conditions_treatment_Treatment = new Attribute("Treatment", "Treatment", "varchar", false, true);
            Attribute conditions_treatment_Medication = new Attribute("Medication", "Medication", "varchar", false, true);
            Attribute conditions_treatment_Immunization = new Attribute("Immunization", "Immunization", "varchar", false, true);
            conditions_treatment.Attributes.Add(conditions_treatment_Prevention);
            conditions_treatment.Attributes.Add(conditions_treatment_Diagnostic_method);
            conditions_treatment.Attributes.Add(conditions_treatment_Treatment);
            conditions_treatment.Attributes.Add(conditions_treatment_Medication);
            conditions_treatment.Attributes.Add(conditions_treatment_Immunization);


            content_conditions.categories.Add(conditions_analysis);
            content_conditions.categories.Add(conditions_causes);
            content_conditions.categories.Add(conditions_effects);
            content_conditions.categories.Add(conditions_gallery);
            content_conditions.categories.Add(conditions_history);
            content_conditions.categories.Add(conditions_notes);
            content_conditions.categories.Add(conditions_overview);
            content_conditions.categories.Add(conditions_treatment);


            Content content_continents = new Content("continents", true);

            Category continents_climate = new Category(5, "Climate", "climate", "climate");

            Attribute continents_climate_Temperature = new Attribute("Temperature", "Temperature", "tinytext", false, true);
            Attribute continents_climate_Seasons = new Attribute("Seasons", "Seasons", "tinytext", false, true);
            Attribute continents_climate_Humidity = new Attribute("Humidity", "Humidity", "tinytext", false, true);
            Attribute continents_climate_Precipitation = new Attribute("Precipitation", "Precipitation", "tinytext", false, true);
            Attribute continents_climate_Winds = new Attribute("Winds", "Winds", "tinytext", false, true);
            Attribute continents_climate_Natural_disasters = new Attribute("Natural_disasters", "Natural Disasters", "tinytext", false, true);
            continents_climate.Attributes.Add(continents_climate_Temperature);
            continents_climate.Attributes.Add(continents_climate_Seasons);
            continents_climate.Attributes.Add(continents_climate_Humidity);
            continents_climate.Attributes.Add(continents_climate_Precipitation);
            continents_climate.Attributes.Add(continents_climate_Winds);
            continents_climate.Attributes.Add(continents_climate_Natural_disasters);


            Category continents_culture = new Category(3, "Culture", "culture", "culture");

            Attribute continents_culture_Demonym = new Attribute("Demonym", "Demonym", "tinytext", false, true);
            Attribute continents_culture_Politics = new Attribute("Politics", "Politics", "tinytext", false, true);
            Attribute continents_culture_Economy = new Attribute("Economy", "Economy", "tinytext", false, true);
            Attribute continents_culture_Tourism = new Attribute("Tourism", "Tourism", "tinytext", false, true);
            Attribute continents_culture_Architecture = new Attribute("Architecture", "Architecture", "tinytext", false, true);
            Attribute continents_culture_Reputation = new Attribute("Reputation", "Reputation", "tinytext", false, true);
            Attribute continents_culture_Countries = new Attribute("Countries", "Countries", "tinytext", false, true);
            Attribute continents_culture_Languages = new Attribute("Languages", "Languages", "tinytext", false, true);
            Attribute continents_culture_Traditions = new Attribute("Traditions", "Traditions", "tinytext", false, true);
            Attribute continents_culture_Governments = new Attribute("Governments", "Governments", "tinytext", false, true);
            Attribute continents_culture_Popular_foods = new Attribute("Popular_foods", "Popular Foods", "tinytext", false, true);
            continents_culture.Attributes.Add(continents_culture_Demonym);
            continents_culture.Attributes.Add(continents_culture_Politics);
            continents_culture.Attributes.Add(continents_culture_Economy);
            continents_culture.Attributes.Add(continents_culture_Tourism);
            continents_culture.Attributes.Add(continents_culture_Architecture);
            continents_culture.Attributes.Add(continents_culture_Reputation);
            continents_culture.Attributes.Add(continents_culture_Countries);
            continents_culture.Attributes.Add(continents_culture_Languages);
            continents_culture.Attributes.Add(continents_culture_Traditions);
            continents_culture.Attributes.Add(continents_culture_Governments);
            continents_culture.Attributes.Add(continents_culture_Popular_foods);


            Category continents_gallery = new Category(7, "Gallery", "gallery", "gallery");



            Category continents_geography = new Category(2, "Geography", "geography", "geography");

            Attribute continents_geography_Area = new Attribute("Area", "Area", "double", false, true);
            Attribute continents_geography_Shape = new Attribute("Shape", "Shape", "tinytext", false, true);
            Attribute continents_geography_Population = new Attribute("Population", "Population", "tinytext", false, true);
            Attribute continents_geography_Topography = new Attribute("Topography", "Topography", "tinytext", false, true);
            Attribute continents_geography_Mineralogy = new Attribute("Mineralogy", "Mineralogy", "tinytext", false, true);
            Attribute continents_geography_Bodies_of_water = new Attribute("Bodies_of_water", "Bodies Of Water", "tinytext", false, true);
            Attribute continents_geography_Landmarks = new Attribute("Landmarks", "Landmarks", "tinytext", false, true);
            Attribute continents_geography_Regional_advantages = new Attribute("Regional_advantages", "Regional Advantages", "tinytext", false, true);
            Attribute continents_geography_Regional_disadvantages = new Attribute("Regional_disadvantages", "Regional Disadvantages", "tinytext", false, true);
            continents_geography.Attributes.Add(continents_geography_Area);
            continents_geography.Attributes.Add(continents_geography_Shape);
            continents_geography.Attributes.Add(continents_geography_Population);
            continents_geography.Attributes.Add(continents_geography_Topography);
            continents_geography.Attributes.Add(continents_geography_Mineralogy);
            continents_geography.Attributes.Add(continents_geography_Bodies_of_water);
            continents_geography.Attributes.Add(continents_geography_Landmarks);
            continents_geography.Attributes.Add(continents_geography_Regional_advantages);
            continents_geography.Attributes.Add(continents_geography_Regional_disadvantages);


            Category continents_history = new Category(6, "History", "history", "history");

            Attribute continents_history_Formation = new Attribute("Formation", "Formation", "tinytext", false, true);
            Attribute continents_history_Discovery = new Attribute("Discovery", "Discovery", "tinytext", false, true);
            Attribute continents_history_Wars = new Attribute("Wars", "Wars", "tinytext", false, true);
            Attribute continents_history_Ruins = new Attribute("Ruins", "Ruins", "tinytext", false, true);
            continents_history.Attributes.Add(continents_history_Formation);
            continents_history.Attributes.Add(continents_history_Discovery);
            continents_history.Attributes.Add(continents_history_Wars);
            continents_history.Attributes.Add(continents_history_Ruins);


            Category continents_nature = new Category(4, "Nature", "nature", "nature");

            Attribute continents_nature_Crops = new Attribute("Crops", "Crops", "tinytext", false, true);
            Attribute continents_nature_Creatures = new Attribute("Creatures", "Creatures", "tinytext", false, true);
            Attribute continents_nature_Floras = new Attribute("Floras", "Floras", "tinytext", false, true);
            continents_nature.Attributes.Add(continents_nature_Crops);
            continents_nature.Attributes.Add(continents_nature_Creatures);
            continents_nature.Attributes.Add(continents_nature_Floras);


            Category continents_notes = new Category(8, "Notes", "notes", "notes");

            Attribute continents_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute continents_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            continents_notes.Attributes.Add(continents_notes_Notes);
            continents_notes.Attributes.Add(continents_notes_Private_Notes);


            Category continents_overview = new Category(1, "Overview", "overview", "overview");

            Attribute continents_overview_Local_name = new Attribute("Local_name", "Local Name", "text", false, true);
            Attribute continents_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute continents_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute continents_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute continents_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            continents_overview.Attributes.Add(continents_overview_Local_name);
            continents_overview.Attributes.Add(continents_overview_Other_Names);
            continents_overview.Attributes.Add(continents_overview_Description);
            continents_overview.Attributes.Add(continents_overview_Universe);
            continents_overview.Attributes.Add(continents_overview_Tags);


            content_continents.categories.Add(continents_climate);
            content_continents.categories.Add(continents_culture);
            content_continents.categories.Add(continents_gallery);
            content_continents.categories.Add(continents_geography);
            content_continents.categories.Add(continents_history);
            content_continents.categories.Add(continents_nature);
            content_continents.categories.Add(continents_notes);
            content_continents.categories.Add(continents_overview);


            Content content_countries = new Content("countries", true);

            Category countries_culture = new Category(3, "Culture", "culture", "culture");

            Attribute countries_culture_Population = new Attribute("Population", "Population", "double", false, true);
            Attribute countries_culture_Social_hierarchy = new Attribute("Social_hierarchy", "Social Hierarchy", "tinytext", false, true);
            Attribute countries_culture_Currency = new Attribute("Currency", "Currency", "varchar", false, true);
            Attribute countries_culture_Laws = new Attribute("Laws", "Laws", "tinytext", false, true);
            Attribute countries_culture_Pop_culture = new Attribute("Pop_culture", "Pop Culture", "tinytext", false, true);
            Attribute countries_culture_Music = new Attribute("Music", "Music", "tinytext", false, true);
            Attribute countries_culture_Education = new Attribute("Education", "Education", "varchar", false, true);
            Attribute countries_culture_Architecture = new Attribute("Architecture", "Architecture", "tinytext", false, true);
            Attribute countries_culture_Sports = new Attribute("Sports", "Sports", "varchar", false, true);
            Attribute countries_culture_Languages = new Attribute("Languages", "Languages", "varchar", false, true);
            Attribute countries_culture_Religions = new Attribute("Religions", "Religions", "varchar", false, true);
            Attribute countries_culture_Governments = new Attribute("Governments", "Governments", "varchar", false, true);
            countries_culture.Attributes.Add(countries_culture_Population);
            countries_culture.Attributes.Add(countries_culture_Social_hierarchy);
            countries_culture.Attributes.Add(countries_culture_Currency);
            countries_culture.Attributes.Add(countries_culture_Laws);
            countries_culture.Attributes.Add(countries_culture_Pop_culture);
            countries_culture.Attributes.Add(countries_culture_Music);
            countries_culture.Attributes.Add(countries_culture_Education);
            countries_culture.Attributes.Add(countries_culture_Architecture);
            countries_culture.Attributes.Add(countries_culture_Sports);
            countries_culture.Attributes.Add(countries_culture_Languages);
            countries_culture.Attributes.Add(countries_culture_Religions);
            countries_culture.Attributes.Add(countries_culture_Governments);


            Category countries_gallery = new Category(6, "Gallery", "gallery", "gallery");



            Category countries_geography = new Category(4, "Geography", "geography", "geography");

            Attribute countries_geography_Area = new Attribute("Area", "Area", "double", false, true);
            Attribute countries_geography_Crops = new Attribute("Crops", "Crops", "tinytext", false, true);
            Attribute countries_geography_Climate = new Attribute("Climate", "Climate", "tinytext", false, true);
            Attribute countries_geography_Creatures = new Attribute("Creatures", "Creatures", "tinytext", false, true);
            Attribute countries_geography_Flora = new Attribute("Flora", "Flora", "tinytext", false, true);
            countries_geography.Attributes.Add(countries_geography_Area);
            countries_geography.Attributes.Add(countries_geography_Crops);
            countries_geography.Attributes.Add(countries_geography_Climate);
            countries_geography.Attributes.Add(countries_geography_Creatures);
            countries_geography.Attributes.Add(countries_geography_Flora);


            Category countries_history = new Category(5, "History", "history", "history");

            Attribute countries_history_Founding_story = new Attribute("Founding_story", "Founding Story", "tinytext", false, true);
            Attribute countries_history_Established_year = new Attribute("Established_year", "Established Year", "int", false, true);
            Attribute countries_history_Notable_wars = new Attribute("Notable_wars", "Notable Wars", "tinytext", false, true);
            countries_history.Attributes.Add(countries_history_Founding_story);
            countries_history.Attributes.Add(countries_history_Established_year);
            countries_history.Attributes.Add(countries_history_Notable_wars);


            Category countries_notes = new Category(7, "Notes", "notes", "notes");

            Attribute countries_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute countries_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            countries_notes.Attributes.Add(countries_notes_Notes);
            countries_notes.Attributes.Add(countries_notes_Private_Notes);


            Category countries_overview = new Category(1, "Overview", "overview", "overview");

            Attribute countries_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute countries_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute countries_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute countries_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute countries_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            countries_overview.Attributes.Add(countries_overview_Name);
            countries_overview.Attributes.Add(countries_overview_Description);
            countries_overview.Attributes.Add(countries_overview_Other_Names);
            countries_overview.Attributes.Add(countries_overview_Universe);
            countries_overview.Attributes.Add(countries_overview_Tags);


            Category countries_points_of_interest = new Category(2, "Points Of Interest", "points_of_interest", "points_of_interest");

            Attribute countries_points_of_interest_Locations = new Attribute("Locations", "Locations", "varchar", false, true);
            Attribute countries_points_of_interest_Towns = new Attribute("Towns", "Towns", "varchar", false, true);
            Attribute countries_points_of_interest_Landmarks = new Attribute("Landmarks", "Landmarks", "varchar", false, true);
            Attribute countries_points_of_interest_Bordering_countries = new Attribute("Bordering_countries", "Bordering Countries", "varchar", false, true);
            countries_points_of_interest.Attributes.Add(countries_points_of_interest_Locations);
            countries_points_of_interest.Attributes.Add(countries_points_of_interest_Towns);
            countries_points_of_interest.Attributes.Add(countries_points_of_interest_Landmarks);
            countries_points_of_interest.Attributes.Add(countries_points_of_interest_Bordering_countries);


            content_countries.categories.Add(countries_culture);
            content_countries.categories.Add(countries_gallery);
            content_countries.categories.Add(countries_geography);
            content_countries.categories.Add(countries_history);
            content_countries.categories.Add(countries_notes);
            content_countries.categories.Add(countries_overview);
            content_countries.categories.Add(countries_points_of_interest);


            Content content_creatures = new Content("creatures", true);

            Category creatures_classification = new Category(8, "Classification", "classification", "classification");

            Attribute creatures_classification_Phylum = new Attribute("Phylum", "Phylum", "tinytext", false, true);
            Attribute creatures_classification_Class = new Attribute("Class", "Class", "tinytext", false, true);
            Attribute creatures_classification_Order = new Attribute("Order", "Order", "tinytext", false, true);
            Attribute creatures_classification_Family = new Attribute("Family", "Family", "tinytext", false, true);
            Attribute creatures_classification_Genus = new Attribute("Genus", "Genus", "tinytext", false, true);
            Attribute creatures_classification_Species = new Attribute("Species", "Species", "tinytext", false, true);
            Attribute creatures_classification_Variations = new Attribute("Variations", "Variations", "tinytext", false, true);
            creatures_classification.Attributes.Add(creatures_classification_Phylum);
            creatures_classification.Attributes.Add(creatures_classification_Class);
            creatures_classification.Attributes.Add(creatures_classification_Order);
            creatures_classification.Attributes.Add(creatures_classification_Family);
            creatures_classification.Attributes.Add(creatures_classification_Genus);
            creatures_classification.Attributes.Add(creatures_classification_Species);
            creatures_classification.Attributes.Add(creatures_classification_Variations);


            Category creatures_comparisons = new Category(5, "Comparisons", "comparisons", "comparisons");

            Attribute creatures_comparisons_Similar_creatures = new Attribute("Similar_creatures", "Similar Creatures", "tinytext", false, true);
            Attribute creatures_comparisons_Symbolisms = new Attribute("Symbolisms", "Symbolisms", "tinytext", false, true);
            Attribute creatures_comparisons_Related_creatures = new Attribute("Related_creatures", "Related Creatures", "tinytext", false, true);
            creatures_comparisons.Attributes.Add(creatures_comparisons_Similar_creatures);
            creatures_comparisons.Attributes.Add(creatures_comparisons_Symbolisms);
            creatures_comparisons.Attributes.Add(creatures_comparisons_Related_creatures);


            Category creatures_evolution = new Category(6, "Evolution", "evolution", "evolution");

            Attribute creatures_evolution_Ancestors = new Attribute("Ancestors", "Ancestors", "tinytext", false, true);
            Attribute creatures_evolution_Evolutionary_drive = new Attribute("Evolutionary_drive", "Evolutionary Drive", "tinytext", false, true);
            Attribute creatures_evolution_Tradeoffs = new Attribute("Tradeoffs", "Tradeoffs", "tinytext", false, true);
            Attribute creatures_evolution_Predictions = new Attribute("Predictions", "Predictions", "tinytext", false, true);
            creatures_evolution.Attributes.Add(creatures_evolution_Ancestors);
            creatures_evolution.Attributes.Add(creatures_evolution_Evolutionary_drive);
            creatures_evolution.Attributes.Add(creatures_evolution_Tradeoffs);
            creatures_evolution.Attributes.Add(creatures_evolution_Predictions);


            Category creatures_gallery = new Category(9, "Gallery", "gallery", "gallery");



            Category creatures_habitat = new Category(4, "Habitat", "habitat", "habitat");

            Attribute creatures_habitat_Preferred_habitat = new Attribute("Preferred_habitat", "Preferred Habitat", "tinytext", false, true);
            Attribute creatures_habitat_Habitats = new Attribute("Habitats", "Habitats", "tinytext", false, true);
            Attribute creatures_habitat_Food_sources = new Attribute("Food_sources", "Food Sources", "tinytext", false, true);
            Attribute creatures_habitat_Migratory_patterns = new Attribute("Migratory_patterns", "Migratory Patterns", "tinytext", false, true);
            Attribute creatures_habitat_Herding_patterns = new Attribute("Herding_patterns", "Herding Patterns", "tinytext", false, true);
            Attribute creatures_habitat_Competitors = new Attribute("Competitors", "Competitors", "tinytext", false, true);
            Attribute creatures_habitat_Predators = new Attribute("Predators", "Predators", "tinytext", false, true);
            Attribute creatures_habitat_Prey = new Attribute("Prey", "Prey", "tinytext", false, true);
            creatures_habitat.Attributes.Add(creatures_habitat_Preferred_habitat);
            creatures_habitat.Attributes.Add(creatures_habitat_Habitats);
            creatures_habitat.Attributes.Add(creatures_habitat_Food_sources);
            creatures_habitat.Attributes.Add(creatures_habitat_Migratory_patterns);
            creatures_habitat.Attributes.Add(creatures_habitat_Herding_patterns);
            creatures_habitat.Attributes.Add(creatures_habitat_Competitors);
            creatures_habitat.Attributes.Add(creatures_habitat_Predators);
            creatures_habitat.Attributes.Add(creatures_habitat_Prey);


            Category creatures_looks = new Category(2, "Looks", "looks", "looks");

            Attribute creatures_looks_Color = new Attribute("Color", "Color", "int", false, true);
            Attribute creatures_looks_Shape = new Attribute("Shape", "Shape", "tinytext", false, true);
            Attribute creatures_looks_Size = new Attribute("Size", "Size", "double", false, true);
            Attribute creatures_looks_Height = new Attribute("Height", "Height", "double", false, true);
            Attribute creatures_looks_Weight = new Attribute("Weight", "Weight", "double", false, true);
            Attribute creatures_looks_Notable_features = new Attribute("Notable_features", "Notable Features", "tinytext", false, true);
            Attribute creatures_looks_Vestigial_features = new Attribute("Vestigial_features", "Vestigial Features", "tinytext", false, true);
            Attribute creatures_looks_Materials = new Attribute("Materials", "Materials", "tinytext", false, true);
            creatures_looks.Attributes.Add(creatures_looks_Color);
            creatures_looks.Attributes.Add(creatures_looks_Shape);
            creatures_looks.Attributes.Add(creatures_looks_Size);
            creatures_looks.Attributes.Add(creatures_looks_Height);
            creatures_looks.Attributes.Add(creatures_looks_Weight);
            creatures_looks.Attributes.Add(creatures_looks_Notable_features);
            creatures_looks.Attributes.Add(creatures_looks_Vestigial_features);
            creatures_looks.Attributes.Add(creatures_looks_Materials);


            Category creatures_notes = new Category(10, "Notes", "notes", "notes");

            Attribute creatures_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute creatures_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", false, true);
            creatures_notes.Attributes.Add(creatures_notes_Notes);
            creatures_notes.Attributes.Add(creatures_notes_Private_notes);


            Category creatures_overview = new Category(1, "Overview", "overview", "overview");

            Attribute creatures_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute creatures_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute creatures_overview_Type_of_creature = new Attribute("Type_of_creature", "Type Of Creature", "text", false, true);
            Attribute creatures_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute creatures_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            creatures_overview.Attributes.Add(creatures_overview_Name);
            creatures_overview.Attributes.Add(creatures_overview_Description);
            creatures_overview.Attributes.Add(creatures_overview_Type_of_creature);
            creatures_overview.Attributes.Add(creatures_overview_Universe);
            creatures_overview.Attributes.Add(creatures_overview_Tags);


            Category creatures_reproduction = new Category(7, "Reproduction", "reproduction", "reproduction");

            Attribute creatures_reproduction_Reproduction_age = new Attribute("Reproduction_age", "Reproduction Age", "double", false, true);
            Attribute creatures_reproduction_Requirements = new Attribute("Requirements", "Requirements", "tinytext", false, true);
            Attribute creatures_reproduction_Mating_ritual = new Attribute("Mating_ritual", "Mating Ritual", "tinytext", false, true);
            Attribute creatures_reproduction_Reproduction = new Attribute("Reproduction", "Reproduction", "tinytext", false, true);
            Attribute creatures_reproduction_Reproduction_frequency = new Attribute("Reproduction_frequency", "Reproduction Frequency", "tinytext", false, true);
            Attribute creatures_reproduction_Parental_instincts = new Attribute("Parental_instincts", "Parental Instincts", "tinytext", false, true);
            Attribute creatures_reproduction_Offspring_care = new Attribute("Offspring_care", "Offspring Care", "tinytext", false, true);
            Attribute creatures_reproduction_Mortality_rate = new Attribute("Mortality_rate", "Mortality Rate", "tinytext", false, true);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Reproduction_age);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Requirements);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Mating_ritual);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Reproduction);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Reproduction_frequency);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Parental_instincts);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Offspring_care);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Mortality_rate);


            Category creatures_traits = new Category(3, "Traits", "traits", "traits");

            Attribute creatures_traits_Aggressiveness = new Attribute("Aggressiveness", "Aggressiveness", "tinytext", false, true);
            Attribute creatures_traits_Method_of_attack = new Attribute("Method_of_attack", "Method Of Attack", "tinytext", false, true);
            Attribute creatures_traits_Methods_of_defense = new Attribute("Methods_of_defense", "Methods Of Defense", "tinytext", false, true);
            Attribute creatures_traits_Maximum_speed = new Attribute("Maximum_speed", "Maximum Speed", "double", false, true);
            Attribute creatures_traits_Strengths = new Attribute("Strengths", "Strengths", "tinytext", false, true);
            Attribute creatures_traits_Weaknesses = new Attribute("Weaknesses", "Weaknesses", "tinytext", false, true);
            Attribute creatures_traits_Sounds = new Attribute("Sounds", "Sounds", "tinytext", false, true);
            Attribute creatures_traits_Spoils = new Attribute("Spoils", "Spoils", "tinytext", false, true);
            Attribute creatures_traits_Conditions = new Attribute("Conditions", "Conditions", "tinytext", false, true);
            Attribute creatures_traits_Weakest_sense = new Attribute("Weakest_sense", "Weakest Sense", "tinytext", false, true);
            Attribute creatures_traits_Strongest_sense = new Attribute("Strongest_sense", "Strongest Sense", "tinytext", false, true);
            creatures_traits.Attributes.Add(creatures_traits_Aggressiveness);
            creatures_traits.Attributes.Add(creatures_traits_Method_of_attack);
            creatures_traits.Attributes.Add(creatures_traits_Methods_of_defense);
            creatures_traits.Attributes.Add(creatures_traits_Maximum_speed);
            creatures_traits.Attributes.Add(creatures_traits_Strengths);
            creatures_traits.Attributes.Add(creatures_traits_Weaknesses);
            creatures_traits.Attributes.Add(creatures_traits_Sounds);
            creatures_traits.Attributes.Add(creatures_traits_Spoils);
            creatures_traits.Attributes.Add(creatures_traits_Conditions);
            creatures_traits.Attributes.Add(creatures_traits_Weakest_sense);
            creatures_traits.Attributes.Add(creatures_traits_Strongest_sense);


            content_creatures.categories.Add(creatures_classification);
            content_creatures.categories.Add(creatures_comparisons);
            content_creatures.categories.Add(creatures_evolution);
            content_creatures.categories.Add(creatures_gallery);
            content_creatures.categories.Add(creatures_habitat);
            content_creatures.categories.Add(creatures_looks);
            content_creatures.categories.Add(creatures_notes);
            content_creatures.categories.Add(creatures_overview);
            content_creatures.categories.Add(creatures_reproduction);
            content_creatures.categories.Add(creatures_traits);


            Content content_deities = new Content("deities", true);

            Category deities_appearance = new Category(2, "Appearance", "appearance", "appearance");

            Attribute deities_appearance_Physical_Description = new Attribute("Physical_Description", "Physical Description", "tinytext", false, true);
            Attribute deities_appearance_Height = new Attribute("Height", "Height", "double", false, true);
            Attribute deities_appearance_Weight = new Attribute("Weight", "Weight", "double", false, true);
            deities_appearance.Attributes.Add(deities_appearance_Physical_Description);
            deities_appearance.Attributes.Add(deities_appearance_Height);
            deities_appearance.Attributes.Add(deities_appearance_Weight);


            Category deities_family = new Category(3, "Family", "family", "family");

            Attribute deities_family_Parents = new Attribute("Parents", "Parents", "tinytext", false, true);
            Attribute deities_family_Partners = new Attribute("Partners", "Partners", "tinytext", false, true);
            Attribute deities_family_Children = new Attribute("Children", "Children", "tinytext", false, true);
            Attribute deities_family_Siblings = new Attribute("Siblings", "Siblings", "tinytext", false, true);
            deities_family.Attributes.Add(deities_family_Parents);
            deities_family.Attributes.Add(deities_family_Partners);
            deities_family.Attributes.Add(deities_family_Children);
            deities_family.Attributes.Add(deities_family_Siblings);


            Category deities_gallery = new Category(8, "Gallery", "gallery", "gallery");



            Category deities_history = new Category(7, "History", "history", "history");

            Attribute deities_history_Notable_Events = new Attribute("Notable_Events", "Notable Events", "tinytext", false, true);
            Attribute deities_history_Family_History = new Attribute("Family_History", "Family History", "tinytext", false, true);
            Attribute deities_history_Life_Story = new Attribute("Life_Story", "Life Story", "text", false, true);
            deities_history.Attributes.Add(deities_history_Notable_Events);
            deities_history.Attributes.Add(deities_history_Family_History);
            deities_history.Attributes.Add(deities_history_Life_Story);


            Category deities_notes = new Category(9, "Notes", "notes", "notes");

            Attribute deities_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute deities_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            deities_notes.Attributes.Add(deities_notes_Notes);
            deities_notes.Attributes.Add(deities_notes_Private_Notes);


            Category deities_overview = new Category(1, "Overview", "overview", "overview");

            Attribute deities_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute deities_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute deities_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute deities_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute deities_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            deities_overview.Attributes.Add(deities_overview_Name);
            deities_overview.Attributes.Add(deities_overview_Description);
            deities_overview.Attributes.Add(deities_overview_Other_Names);
            deities_overview.Attributes.Add(deities_overview_Universe);
            deities_overview.Attributes.Add(deities_overview_Tags);


            Category deities_powers = new Category(5, "Powers", "powers", "powers");

            Attribute deities_powers_Strengths = new Attribute("Strengths", "Strengths", "tinytext", false, true);
            Attribute deities_powers_Weaknesses = new Attribute("Weaknesses", "Weaknesses", "tinytext", false, true);
            Attribute deities_powers_Abilities = new Attribute("Abilities", "Abilities", "tinytext", false, true);
            Attribute deities_powers_Conditions = new Attribute("Conditions", "Conditions", "tinytext", false, true);
            deities_powers.Attributes.Add(deities_powers_Strengths);
            deities_powers.Attributes.Add(deities_powers_Weaknesses);
            deities_powers.Attributes.Add(deities_powers_Abilities);
            deities_powers.Attributes.Add(deities_powers_Conditions);


            Category deities_rituals = new Category(6, "Rituals", "rituals", "rituals");

            Attribute deities_rituals_Prayers = new Attribute("Prayers", "Prayers", "tinytext", false, true);
            Attribute deities_rituals_Rituals = new Attribute("Rituals", "Rituals", "tinytext", false, true);
            Attribute deities_rituals_Traditions = new Attribute("Traditions", "Traditions", "tinytext", false, true);
            Attribute deities_rituals_Human_Interaction = new Attribute("Human_Interaction", "Human Interaction", "tinytext", false, true);
            Attribute deities_rituals_Related_towns = new Attribute("Related_towns", "Related Towns", "tinytext", false, true);
            Attribute deities_rituals_Related_races = new Attribute("Related_races", "Related Races", "tinytext", false, true);
            Attribute deities_rituals_Related_landmarks = new Attribute("Related_landmarks", "Related Landmarks", "tinytext", false, true);
            deities_rituals.Attributes.Add(deities_rituals_Prayers);
            deities_rituals.Attributes.Add(deities_rituals_Rituals);
            deities_rituals.Attributes.Add(deities_rituals_Traditions);
            deities_rituals.Attributes.Add(deities_rituals_Human_Interaction);
            deities_rituals.Attributes.Add(deities_rituals_Related_towns);
            deities_rituals.Attributes.Add(deities_rituals_Related_races);
            deities_rituals.Attributes.Add(deities_rituals_Related_landmarks);


            Category deities_symbolism = new Category(4, "Symbolism", "symbolism", "symbolism");

            Attribute deities_symbolism_Symbols = new Attribute("Symbols", "Symbols", "tinytext", false, true);
            Attribute deities_symbolism_Elements = new Attribute("Elements", "Elements", "tinytext", false, true);
            Attribute deities_symbolism_Creatures = new Attribute("Creatures", "Creatures", "tinytext", false, true);
            Attribute deities_symbolism_Floras = new Attribute("Floras", "Floras", "tinytext", false, true);
            Attribute deities_symbolism_Religions = new Attribute("Religions", "Religions", "tinytext", false, true);
            Attribute deities_symbolism_Relics = new Attribute("Relics", "Relics", "tinytext", false, true);
            deities_symbolism.Attributes.Add(deities_symbolism_Symbols);
            deities_symbolism.Attributes.Add(deities_symbolism_Elements);
            deities_symbolism.Attributes.Add(deities_symbolism_Creatures);
            deities_symbolism.Attributes.Add(deities_symbolism_Floras);
            deities_symbolism.Attributes.Add(deities_symbolism_Religions);
            deities_symbolism.Attributes.Add(deities_symbolism_Relics);


            content_deities.categories.Add(deities_appearance);
            content_deities.categories.Add(deities_family);
            content_deities.categories.Add(deities_gallery);
            content_deities.categories.Add(deities_history);
            content_deities.categories.Add(deities_notes);
            content_deities.categories.Add(deities_overview);
            content_deities.categories.Add(deities_powers);
            content_deities.categories.Add(deities_rituals);
            content_deities.categories.Add(deities_symbolism);


            Content content_floras = new Content("floras", true);

            Category floras_appearance = new Category(3, "Appearance", "appearance", "appearance");

            Attribute floras_appearance_Size = new Attribute("Size", "Size", "varchar", false, true);
            Attribute floras_appearance_Smell = new Attribute("Smell", "Smell", "varchar", false, true);
            Attribute floras_appearance_Taste = new Attribute("Taste", "Taste", "varchar", false, true);
            Attribute floras_appearance_Colorings = new Attribute("Colorings", "Colorings", "varchar", false, true);
            floras_appearance.Attributes.Add(floras_appearance_Size);
            floras_appearance.Attributes.Add(floras_appearance_Smell);
            floras_appearance.Attributes.Add(floras_appearance_Taste);
            floras_appearance.Attributes.Add(floras_appearance_Colorings);


            Category floras_classification = new Category(2, "Classification", "classification", "classification");

            Attribute floras_classification_Order = new Attribute("Order", "Order", "varchar", false, true);
            Attribute floras_classification_Family = new Attribute("Family", "Family", "varchar", false, true);
            Attribute floras_classification_Genus = new Attribute("Genus", "Genus", "varchar", false, true);
            Attribute floras_classification_Related_flora = new Attribute("Related_flora", "Related Flora", "varchar", false, true);
            floras_classification.Attributes.Add(floras_classification_Order);
            floras_classification.Attributes.Add(floras_classification_Family);
            floras_classification.Attributes.Add(floras_classification_Genus);
            floras_classification.Attributes.Add(floras_classification_Related_flora);


            Category floras_ecosystem = new Category(5, "Ecosystem", "ecosystem", "ecosystem");

            Attribute floras_ecosystem_Locations = new Attribute("Locations", "Locations", "tinytext", false, true);
            Attribute floras_ecosystem_Reproduction = new Attribute("Reproduction", "Reproduction", "tinytext", false, true);
            Attribute floras_ecosystem_Seasonality = new Attribute("Seasonality", "Seasonality", "tinytext", false, true);
            Attribute floras_ecosystem_Eaten_by = new Attribute("Eaten_by", "Eaten By", "tinytext", false, true);
            floras_ecosystem.Attributes.Add(floras_ecosystem_Locations);
            floras_ecosystem.Attributes.Add(floras_ecosystem_Reproduction);
            floras_ecosystem.Attributes.Add(floras_ecosystem_Seasonality);
            floras_ecosystem.Attributes.Add(floras_ecosystem_Eaten_by);


            Category floras_gallery = new Category(6, "Gallery", "gallery", "gallery");



            Category floras_notes = new Category(7, "Notes", "notes", "notes");

            Attribute floras_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute floras_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            floras_notes.Attributes.Add(floras_notes_Notes);
            floras_notes.Attributes.Add(floras_notes_Private_Notes);


            Category floras_overview = new Category(1, "Overview", "overview", "overview");

            Attribute floras_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute floras_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute floras_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute floras_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute floras_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            floras_overview.Attributes.Add(floras_overview_Name);
            floras_overview.Attributes.Add(floras_overview_Description);
            floras_overview.Attributes.Add(floras_overview_Other_Names);
            floras_overview.Attributes.Add(floras_overview_Universe);
            floras_overview.Attributes.Add(floras_overview_Tags);


            Category floras_produce = new Category(4, "Produce", "produce", "produce");

            Attribute floras_produce_Fruits = new Attribute("Fruits", "Fruits", "varchar", false, true);
            Attribute floras_produce_Magical_effects = new Attribute("Magical_effects", "Magical Effects", "tinytext", false, true);
            Attribute floras_produce_Material_uses = new Attribute("Material_uses", "Material Uses", "tinytext", false, true);
            Attribute floras_produce_Medicinal_purposes = new Attribute("Medicinal_purposes", "Medicinal Purposes", "tinytext", false, true);
            Attribute floras_produce_Berries = new Attribute("Berries", "Berries", "tinytext", false, true);
            Attribute floras_produce_Nuts = new Attribute("Nuts", "Nuts", "varchar", false, true);
            Attribute floras_produce_Seeds = new Attribute("Seeds", "Seeds", "varchar", false, true);
            floras_produce.Attributes.Add(floras_produce_Fruits);
            floras_produce.Attributes.Add(floras_produce_Magical_effects);
            floras_produce.Attributes.Add(floras_produce_Material_uses);
            floras_produce.Attributes.Add(floras_produce_Medicinal_purposes);
            floras_produce.Attributes.Add(floras_produce_Berries);
            floras_produce.Attributes.Add(floras_produce_Nuts);
            floras_produce.Attributes.Add(floras_produce_Seeds);


            content_floras.categories.Add(floras_appearance);
            content_floras.categories.Add(floras_classification);
            content_floras.categories.Add(floras_ecosystem);
            content_floras.categories.Add(floras_gallery);
            content_floras.categories.Add(floras_notes);
            content_floras.categories.Add(floras_overview);
            content_floras.categories.Add(floras_produce);


            Content content_foods = new Content("foods", true);

            Category foods_changelog = new Category(8, "Changelog", "changelog", "changelog");



            Category foods_eating = new Category(4, "Eating", "eating", "eating");

            Attribute foods_eating_Meal = new Attribute("Meal", "Meal", "tinytext", false, true);
            Attribute foods_eating_Serving = new Attribute("Serving", "Serving", "tinytext", false, true);
            Attribute foods_eating_Utensils_needed = new Attribute("Utensils_needed", "Utensils Needed", "tinytext", false, true);
            Attribute foods_eating_Texture = new Attribute("Texture", "Texture", "tinytext", false, true);
            Attribute foods_eating_Scent = new Attribute("Scent", "Scent", "tinytext", false, true);
            Attribute foods_eating_Flavor = new Attribute("Flavor", "Flavor", "tinytext", false, true);
            foods_eating.Attributes.Add(foods_eating_Meal);
            foods_eating.Attributes.Add(foods_eating_Serving);
            foods_eating.Attributes.Add(foods_eating_Utensils_needed);
            foods_eating.Attributes.Add(foods_eating_Texture);
            foods_eating.Attributes.Add(foods_eating_Scent);
            foods_eating.Attributes.Add(foods_eating_Flavor);


            Category foods_effects = new Category(5, "Effects", "effects", "effects");

            Attribute foods_effects_Nutrition = new Attribute("Nutrition", "Nutrition", "tinytext", false, true);
            Attribute foods_effects_Conditions = new Attribute("Conditions", "Conditions", "tinytext", false, true);
            Attribute foods_effects_Side_effects = new Attribute("Side_effects", "Side Effects", "tinytext", false, true);
            foods_effects.Attributes.Add(foods_effects_Nutrition);
            foods_effects.Attributes.Add(foods_effects_Conditions);
            foods_effects.Attributes.Add(foods_effects_Side_effects);


            Category foods_gallery = new Category(7, "Gallery", "gallery", "gallery");



            Category foods_history = new Category(6, "History", "history", "history");

            Attribute foods_history_Place_of_origin = new Attribute("Place_of_origin", "Place Of Origin", "tinytext", false, true);
            Attribute foods_history_Origin_story = new Attribute("Origin_story", "Origin Story", "tinytext", false, true);
            Attribute foods_history_Traditions = new Attribute("Traditions", "Traditions", "tinytext", false, true);
            Attribute foods_history_Symbolisms = new Attribute("Symbolisms", "Symbolisms", "tinytext", false, true);
            Attribute foods_history_Related_foods = new Attribute("Related_foods", "Related Foods", "tinytext", false, true);
            Attribute foods_history_Reputation = new Attribute("Reputation", "Reputation", "tinytext", false, true);
            foods_history.Attributes.Add(foods_history_Place_of_origin);
            foods_history.Attributes.Add(foods_history_Origin_story);
            foods_history.Attributes.Add(foods_history_Traditions);
            foods_history.Attributes.Add(foods_history_Symbolisms);
            foods_history.Attributes.Add(foods_history_Related_foods);
            foods_history.Attributes.Add(foods_history_Reputation);


            Category foods_notes = new Category(9, "Notes", "notes", "notes");

            Attribute foods_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute foods_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            foods_notes.Attributes.Add(foods_notes_Notes);
            foods_notes.Attributes.Add(foods_notes_Private_Notes);


            Category foods_overview = new Category(1, "Overview", "overview", "overview");

            Attribute foods_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute foods_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute foods_overview_Type_of_food = new Attribute("Type_of_food", "Type Of Food", "text", false, true);
            Attribute foods_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute foods_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute foods_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            foods_overview.Attributes.Add(foods_overview_Name);
            foods_overview.Attributes.Add(foods_overview_Description);
            foods_overview.Attributes.Add(foods_overview_Type_of_food);
            foods_overview.Attributes.Add(foods_overview_Other_Names);
            foods_overview.Attributes.Add(foods_overview_Universe);
            foods_overview.Attributes.Add(foods_overview_Tags);


            Category foods_recipe = new Category(2, "Recipe", "recipe", "recipe");

            Attribute foods_recipe_Ingredients = new Attribute("Ingredients", "Ingredients", "tinytext", false, true);
            Attribute foods_recipe_Preparation = new Attribute("Preparation", "Preparation", "tinytext", false, true);
            Attribute foods_recipe_Cooking_method = new Attribute("Cooking_method", "Cooking Method", "tinytext", false, true);
            Attribute foods_recipe_Spices = new Attribute("Spices", "Spices", "tinytext", false, true);
            Attribute foods_recipe_Yield = new Attribute("Yield", "Yield", "tinytext", false, true);
            Attribute foods_recipe_Color = new Attribute("Color", "Color", "varchar", false, true);
            Attribute foods_recipe_Size = new Attribute("Size", "Size", "double", false, true);
            Attribute foods_recipe_Variations = new Attribute("Variations", "Variations", "tinytext", false, true);
            Attribute foods_recipe_Smell = new Attribute("Smell", "Smell", "varchar", false, true);
            foods_recipe.Attributes.Add(foods_recipe_Ingredients);
            foods_recipe.Attributes.Add(foods_recipe_Preparation);
            foods_recipe.Attributes.Add(foods_recipe_Cooking_method);
            foods_recipe.Attributes.Add(foods_recipe_Spices);
            foods_recipe.Attributes.Add(foods_recipe_Yield);
            foods_recipe.Attributes.Add(foods_recipe_Color);
            foods_recipe.Attributes.Add(foods_recipe_Size);
            foods_recipe.Attributes.Add(foods_recipe_Variations);
            foods_recipe.Attributes.Add(foods_recipe_Smell);


            Category foods_sales = new Category(3, "Sales", "sales", "sales");

            Attribute foods_sales_Cost = new Attribute("Cost", "Cost", "tinytext", false, true);
            Attribute foods_sales_Sold_by = new Attribute("Sold_by", "Sold By", "tinytext", false, true);
            Attribute foods_sales_Rarity = new Attribute("Rarity", "Rarity", "tinytext", false, true);
            Attribute foods_sales_Shelf_life = new Attribute("Shelf_life", "Shelf Life", "tinytext", false, true);
            foods_sales.Attributes.Add(foods_sales_Cost);
            foods_sales.Attributes.Add(foods_sales_Sold_by);
            foods_sales.Attributes.Add(foods_sales_Rarity);
            foods_sales.Attributes.Add(foods_sales_Shelf_life);


            content_foods.categories.Add(foods_changelog);
            content_foods.categories.Add(foods_eating);
            content_foods.categories.Add(foods_effects);
            content_foods.categories.Add(foods_gallery);
            content_foods.categories.Add(foods_history);
            content_foods.categories.Add(foods_notes);
            content_foods.categories.Add(foods_overview);
            content_foods.categories.Add(foods_recipe);
            content_foods.categories.Add(foods_sales);


            Content content_governments = new Content("governments", true);

            Category governments_assets = new Category(8, "Assets", "assets", "assets");

            Attribute governments_assets_Items = new Attribute("Items", "Items", "tinytext", false, true);
            Attribute governments_assets_Technologies = new Attribute("Technologies", "Technologies", "tinytext", false, true);
            Attribute governments_assets_Creatures = new Attribute("Creatures", "Creatures", "tinytext", false, true);
            Attribute governments_assets_Vehicles = new Attribute("Vehicles", "Vehicles", "tinytext", false, true);
            governments_assets.Attributes.Add(governments_assets_Items);
            governments_assets.Attributes.Add(governments_assets_Technologies);
            governments_assets.Attributes.Add(governments_assets_Creatures);
            governments_assets.Attributes.Add(governments_assets_Vehicles);


            Category governments_gallery = new Category(9, "Gallery", "gallery", "gallery");



            Category governments_history = new Category(7, "History", "history", "history");

            Attribute governments_history_Founding_Story = new Attribute("Founding_Story", "Founding Story", "tinytext", false, true);
            Attribute governments_history_Flag_Design_Story = new Attribute("Flag_Design_Story", "Flag Design Story", "tinytext", false, true);
            Attribute governments_history_Notable_Wars = new Attribute("Notable_Wars", "Notable Wars", "tinytext", false, true);
            Attribute governments_history_Holidays = new Attribute("Holidays", "Holidays", "tinytext", false, true);
            governments_history.Attributes.Add(governments_history_Founding_Story);
            governments_history.Attributes.Add(governments_history_Flag_Design_Story);
            governments_history.Attributes.Add(governments_history_Notable_Wars);
            governments_history.Attributes.Add(governments_history_Holidays);


            Category governments_ideologies = new Category(3, "Ideologies", "ideologies", "ideologies");

            Attribute governments_ideologies_Sociopolitical = new Attribute("Sociopolitical", "Sociopolitical", "tinytext", false, true);
            Attribute governments_ideologies_Socioeconomical = new Attribute("Socioeconomical", "Socioeconomical", "tinytext", false, true);
            Attribute governments_ideologies_Geocultural = new Attribute("Geocultural", "Geocultural", "tinytext", false, true);
            Attribute governments_ideologies_Laws = new Attribute("Laws", "Laws", "tinytext", false, true);
            Attribute governments_ideologies_Immigration = new Attribute("Immigration", "Immigration", "tinytext", false, true);
            Attribute governments_ideologies_Privacy_Ideologies = new Attribute("Privacy_Ideologies", "Privacy Ideologies", "tinytext", false, true);
            governments_ideologies.Attributes.Add(governments_ideologies_Sociopolitical);
            governments_ideologies.Attributes.Add(governments_ideologies_Socioeconomical);
            governments_ideologies.Attributes.Add(governments_ideologies_Geocultural);
            governments_ideologies.Attributes.Add(governments_ideologies_Laws);
            governments_ideologies.Attributes.Add(governments_ideologies_Immigration);
            governments_ideologies.Attributes.Add(governments_ideologies_Privacy_Ideologies);


            Category governments_members = new Category(6, "Members", "members", "members");

            Attribute governments_members_Leaders = new Attribute("Leaders", "Leaders", "tinytext", false, true);
            Attribute governments_members_Groups = new Attribute("Groups", "Groups", "tinytext", false, true);
            Attribute governments_members_Political_figures = new Attribute("Political_figures", "Political Figures", "tinytext", false, true);
            Attribute governments_members_Military = new Attribute("Military", "Military", "tinytext", false, true);
            Attribute governments_members_Navy = new Attribute("Navy", "Navy", "tinytext", false, true);
            Attribute governments_members_Airforce = new Attribute("Airforce", "Airforce", "tinytext", false, true);
            Attribute governments_members_Space_Program = new Attribute("Space_Program", "Space Program", "tinytext", false, true);
            governments_members.Attributes.Add(governments_members_Leaders);
            governments_members.Attributes.Add(governments_members_Groups);
            governments_members.Attributes.Add(governments_members_Political_figures);
            governments_members.Attributes.Add(governments_members_Military);
            governments_members.Attributes.Add(governments_members_Navy);
            governments_members.Attributes.Add(governments_members_Airforce);
            governments_members.Attributes.Add(governments_members_Space_Program);


            Category governments_notes = new Category(10, "Notes", "notes", "notes");

            Attribute governments_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute governments_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            governments_notes.Attributes.Add(governments_notes_Notes);
            governments_notes.Attributes.Add(governments_notes_Private_Notes);


            Category governments_overview = new Category(1, "Overview", "overview", "overview");

            Attribute governments_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute governments_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute governments_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute governments_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            governments_overview.Attributes.Add(governments_overview_Name);
            governments_overview.Attributes.Add(governments_overview_Description);
            governments_overview.Attributes.Add(governments_overview_Universe);
            governments_overview.Attributes.Add(governments_overview_Tags);


            Category governments_populace = new Category(5, "Populace", "populace", "populace");

            Attribute governments_populace_Approval_Ratings = new Attribute("Approval_Ratings", "Approval Ratings", "tinytext", false, true);
            Attribute governments_populace_International_Relations = new Attribute("International_Relations", "International Relations", "tinytext", false, true);
            Attribute governments_populace_Civilian_Life = new Attribute("Civilian_Life", "Civilian Life", "tinytext", false, true);
            governments_populace.Attributes.Add(governments_populace_Approval_Ratings);
            governments_populace.Attributes.Add(governments_populace_International_Relations);
            governments_populace.Attributes.Add(governments_populace_Civilian_Life);


            Category governments_process = new Category(4, "Process", "process", "process");

            Attribute governments_process_Electoral_Process = new Attribute("Electoral_Process", "Electoral Process", "tinytext", false, true);
            Attribute governments_process_Term_Lengths = new Attribute("Term_Lengths", "Term Lengths", "tinytext", false, true);
            Attribute governments_process_Criminal_System = new Attribute("Criminal_System", "Criminal System", "tinytext", false, true);
            governments_process.Attributes.Add(governments_process_Electoral_Process);
            governments_process.Attributes.Add(governments_process_Term_Lengths);
            governments_process.Attributes.Add(governments_process_Criminal_System);


            Category governments_structure = new Category(2, "Structure", "structure", "structure");

            Attribute governments_structure_Type_Of_Government = new Attribute("Type_Of_Government", "Type Of Government", "varchar", false, true);
            Attribute governments_structure_Power_Structure = new Attribute("Power_Structure", "Power Structure", "tinytext", false, true);
            Attribute governments_structure_Power_Source = new Attribute("Power_Source", "Power Source", "varchar", false, true);
            Attribute governments_structure_Checks_And_Balances = new Attribute("Checks_And_Balances", "Checks And Balances", "tinytext", false, true);
            Attribute governments_structure_Jobs = new Attribute("Jobs", "Jobs", "tinytext", false, true);
            governments_structure.Attributes.Add(governments_structure_Type_Of_Government);
            governments_structure.Attributes.Add(governments_structure_Power_Structure);
            governments_structure.Attributes.Add(governments_structure_Power_Source);
            governments_structure.Attributes.Add(governments_structure_Checks_And_Balances);
            governments_structure.Attributes.Add(governments_structure_Jobs);


            content_governments.categories.Add(governments_assets);
            content_governments.categories.Add(governments_gallery);
            content_governments.categories.Add(governments_history);
            content_governments.categories.Add(governments_ideologies);
            content_governments.categories.Add(governments_members);
            content_governments.categories.Add(governments_notes);
            content_governments.categories.Add(governments_overview);
            content_governments.categories.Add(governments_populace);
            content_governments.categories.Add(governments_process);
            content_governments.categories.Add(governments_structure);


            Content content_groups = new Content("groups", true);

            Category groups_gallery = new Category(8, "Gallery", "gallery", "gallery");



            Category groups_hierarchy = new Category(7, "Hierarchy", "hierarchy", "hierarchy");

            Attribute groups_hierarchy_Supergroups = new Attribute("Supergroups", "Supergroups", "varchar", false, true);
            Attribute groups_hierarchy_Subgroups = new Attribute("Subgroups", "Subgroups", "varchar", false, true);
            Attribute groups_hierarchy_Sistergroups = new Attribute("Sistergroups", "Sistergroups", "varchar", false, true);
            Attribute groups_hierarchy_Organization_structure = new Attribute("Organization_structure", "Organization Structure", "tinytext", false, true);
            groups_hierarchy.Attributes.Add(groups_hierarchy_Supergroups);
            groups_hierarchy.Attributes.Add(groups_hierarchy_Subgroups);
            groups_hierarchy.Attributes.Add(groups_hierarchy_Sistergroups);
            groups_hierarchy.Attributes.Add(groups_hierarchy_Organization_structure);


            Category groups_inventory = new Category(6, "Inventory", "inventory", "inventory");

            Attribute groups_inventory_Inventory = new Attribute("Inventory", "Inventory", "varchar", false, true);
            Attribute groups_inventory_Equipment = new Attribute("Equipment", "Equipment", "varchar", false, true);
            Attribute groups_inventory_Key_items = new Attribute("Key_items", "Key Items", "varchar", false, true);
            Attribute groups_inventory_Suppliers = new Attribute("Suppliers", "Suppliers", "varchar", false, true);
            groups_inventory.Attributes.Add(groups_inventory_Inventory);
            groups_inventory.Attributes.Add(groups_inventory_Equipment);
            groups_inventory.Attributes.Add(groups_inventory_Key_items);
            groups_inventory.Attributes.Add(groups_inventory_Suppliers);


            Category groups_locations = new Category(3, "Locations", "locations", "locations");

            Attribute groups_locations_Locations = new Attribute("Locations", "Locations", "varchar", false, true);
            Attribute groups_locations_Headquarters = new Attribute("Headquarters", "Headquarters", "varchar", false, true);
            Attribute groups_locations_Offices = new Attribute("Offices", "Offices", "varchar", false, true);
            groups_locations.Attributes.Add(groups_locations_Locations);
            groups_locations.Attributes.Add(groups_locations_Headquarters);
            groups_locations.Attributes.Add(groups_locations_Offices);


            Category groups_members = new Category(2, "Members", "members", "members");

            Attribute groups_members_Leaders = new Attribute("Leaders", "Leaders", "varchar", false, true);
            Attribute groups_members_Creatures = new Attribute("Creatures", "Creatures", "varchar", false, true);
            Attribute groups_members_Members = new Attribute("Members", "Members", "varchar", false, true);
            groups_members.Attributes.Add(groups_members_Leaders);
            groups_members.Attributes.Add(groups_members_Creatures);
            groups_members.Attributes.Add(groups_members_Members);


            Category groups_notes = new Category(9, "Notes", "notes", "notes");

            Attribute groups_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute groups_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", false, true);
            groups_notes.Attributes.Add(groups_notes_Notes);
            groups_notes.Attributes.Add(groups_notes_Private_notes);


            Category groups_overview = new Category(1, "Overview", "overview", "overview");

            Attribute groups_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute groups_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute groups_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute groups_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute groups_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            groups_overview.Attributes.Add(groups_overview_Name);
            groups_overview.Attributes.Add(groups_overview_Description);
            groups_overview.Attributes.Add(groups_overview_Other_Names);
            groups_overview.Attributes.Add(groups_overview_Universe);
            groups_overview.Attributes.Add(groups_overview_Tags);


            Category groups_politics = new Category(5, "Politics", "politics", "politics");

            Attribute groups_politics_Allies = new Attribute("Allies", "Allies", "varchar", false, true);
            Attribute groups_politics_Enemies = new Attribute("Enemies", "Enemies", "varchar", false, true);
            Attribute groups_politics_Rivals = new Attribute("Rivals", "Rivals", "varchar", false, true);
            Attribute groups_politics_Clients = new Attribute("Clients", "Clients", "varchar", false, true);
            groups_politics.Attributes.Add(groups_politics_Allies);
            groups_politics.Attributes.Add(groups_politics_Enemies);
            groups_politics.Attributes.Add(groups_politics_Rivals);
            groups_politics.Attributes.Add(groups_politics_Clients);


            Category groups_purpose = new Category(4, "Purpose", "purpose", "purpose");

            Attribute groups_purpose_Motivations = new Attribute("Motivations", "Motivations", "varchar", false, true);
            Attribute groups_purpose_Goals = new Attribute("Goals", "Goals", "varchar", false, true);
            Attribute groups_purpose_Obstacles = new Attribute("Obstacles", "Obstacles", "varchar", false, true);
            Attribute groups_purpose_Risks = new Attribute("Risks", "Risks", "varchar", false, true);
            Attribute groups_purpose_Traditions = new Attribute("Traditions", "Traditions", "varchar", false, true);
            groups_purpose.Attributes.Add(groups_purpose_Motivations);
            groups_purpose.Attributes.Add(groups_purpose_Goals);
            groups_purpose.Attributes.Add(groups_purpose_Obstacles);
            groups_purpose.Attributes.Add(groups_purpose_Risks);
            groups_purpose.Attributes.Add(groups_purpose_Traditions);


            content_groups.categories.Add(groups_gallery);
            content_groups.categories.Add(groups_hierarchy);
            content_groups.categories.Add(groups_inventory);
            content_groups.categories.Add(groups_locations);
            content_groups.categories.Add(groups_members);
            content_groups.categories.Add(groups_notes);
            content_groups.categories.Add(groups_overview);
            content_groups.categories.Add(groups_politics);
            content_groups.categories.Add(groups_purpose);


            Content content_items = new Content("items", true);

            Category items_abilities = new Category(4, "Abilities", "abilities", "abilities");

            Attribute items_abilities_Magical_effects = new Attribute("Magical_effects", "Magical Effects", "tinytext", false, true);
            Attribute items_abilities_Magic = new Attribute("Magic", "Magic", "varchar", false, true);
            items_abilities.Attributes.Add(items_abilities_Magical_effects);
            items_abilities.Attributes.Add(items_abilities_Magic);


            Category items_gallery = new Category(5, "Gallery", "gallery", "gallery");



            Category items_history = new Category(3, "History", "history", "history");

            Attribute items_history_Original_Owners = new Attribute("Original_Owners", "Original Owners", "varchar", false, true);
            Attribute items_history_Past_Owners = new Attribute("Past_Owners", "Past Owners", "tinytext", false, true);
            Attribute items_history_Current_Owners = new Attribute("Current_Owners", "Current Owners", "varchar", false, true);
            Attribute items_history_Makers = new Attribute("Makers", "Makers", "varchar", false, true);
            Attribute items_history_Year_it_was_made = new Attribute("Year_it_was_made", "Year It Was Made", "int", false, true);
            items_history.Attributes.Add(items_history_Original_Owners);
            items_history.Attributes.Add(items_history_Past_Owners);
            items_history.Attributes.Add(items_history_Current_Owners);
            items_history.Attributes.Add(items_history_Makers);
            items_history.Attributes.Add(items_history_Year_it_was_made);


            Category items_looks = new Category(2, "Looks", "looks", "looks");

            Attribute items_looks_Materials = new Attribute("Materials", "Materials", "varchar", false, true);
            Attribute items_looks_Weight = new Attribute("Weight", "Weight", "double", false, true);
            items_looks.Attributes.Add(items_looks_Materials);
            items_looks.Attributes.Add(items_looks_Weight);


            Category items_notes = new Category(6, "Notes", "notes", "notes");

            Attribute items_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute items_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            items_notes.Attributes.Add(items_notes_Notes);
            items_notes.Attributes.Add(items_notes_Private_Notes);


            Category items_overview = new Category(1, "Overview", "overview", "overview");

            Attribute items_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute items_overview_Item_Type = new Attribute("Item_Type", "Item Type", "varchar", false, true);
            Attribute items_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute items_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute items_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            Attribute items_overview_Technical_effects = new Attribute("Technical_effects", "Technical Effects", "tinytext", false, true);
            Attribute items_overview_Technology = new Attribute("Technology", "Technology", "varchar", false, true);
            items_overview.Attributes.Add(items_overview_Name);
            items_overview.Attributes.Add(items_overview_Item_Type);
            items_overview.Attributes.Add(items_overview_Universe);
            items_overview.Attributes.Add(items_overview_Description);
            items_overview.Attributes.Add(items_overview_Tags);
            items_overview.Attributes.Add(items_overview_Technical_effects);
            items_overview.Attributes.Add(items_overview_Technology);


            content_items.categories.Add(items_abilities);
            content_items.categories.Add(items_gallery);
            content_items.categories.Add(items_history);
            content_items.categories.Add(items_looks);
            content_items.categories.Add(items_notes);
            content_items.categories.Add(items_overview);


            Content content_jobs = new Content("jobs", true);

            Category jobs_gallery = new Category(7, "Gallery", "gallery", "gallery");



            Category jobs_history = new Category(6, "History", "history", "history");

            Attribute jobs_history_Job_origin = new Attribute("Job_origin", "Job Origin", "tinytext", false, true);
            Attribute jobs_history_Initial_goal = new Attribute("Initial_goal", "Initial Goal", "varchar", false, true);
            Attribute jobs_history_Notable_figures = new Attribute("Notable_figures", "Notable Figures", "varchar", false, true);
            Attribute jobs_history_Traditions = new Attribute("Traditions", "Traditions", "tinytext", false, true);
            jobs_history.Attributes.Add(jobs_history_Job_origin);
            jobs_history.Attributes.Add(jobs_history_Initial_goal);
            jobs_history.Attributes.Add(jobs_history_Notable_figures);
            jobs_history.Attributes.Add(jobs_history_Traditions);


            Category jobs_notes = new Category(8, "Notes", "notes", "notes");

            Attribute jobs_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute jobs_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            jobs_notes.Attributes.Add(jobs_notes_Notes);
            jobs_notes.Attributes.Add(jobs_notes_Private_Notes);


            Category jobs_overview = new Category(1, "Overview", "overview", "overview");

            Attribute jobs_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute jobs_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute jobs_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute jobs_overview_Type_of_job = new Attribute("Type_of_job", "Type Of Job", "varchar", false, true);
            Attribute jobs_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "text", false, true);
            Attribute jobs_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            jobs_overview.Attributes.Add(jobs_overview_Name);
            jobs_overview.Attributes.Add(jobs_overview_Universe);
            jobs_overview.Attributes.Add(jobs_overview_Description);
            jobs_overview.Attributes.Add(jobs_overview_Type_of_job);
            jobs_overview.Attributes.Add(jobs_overview_Alternate_names);
            jobs_overview.Attributes.Add(jobs_overview_Tags);


            Category jobs_requirements = new Category(2, "Requirements", "requirements", "requirements");

            Attribute jobs_requirements_Education = new Attribute("Education", "Education", "varchar", false, true);
            Attribute jobs_requirements_Experience = new Attribute("Experience", "Experience", "varchar", false, true);
            Attribute jobs_requirements_Training = new Attribute("Training", "Training", "varchar", false, true);
            Attribute jobs_requirements_Work_hours = new Attribute("Work_hours", "Work Hours", "double", false, true);
            Attribute jobs_requirements_Vehicles = new Attribute("Vehicles", "Vehicles", "varchar", false, true);
            jobs_requirements.Attributes.Add(jobs_requirements_Education);
            jobs_requirements.Attributes.Add(jobs_requirements_Experience);
            jobs_requirements.Attributes.Add(jobs_requirements_Training);
            jobs_requirements.Attributes.Add(jobs_requirements_Work_hours);
            jobs_requirements.Attributes.Add(jobs_requirements_Vehicles);


            Category jobs_rewards = new Category(4, "Rewards", "rewards", "rewards");

            Attribute jobs_rewards_Pay_rate = new Attribute("Pay_rate", "Pay Rate", "double", false, true);
            Attribute jobs_rewards_Time_off = new Attribute("Time_off", "Time Off", "varchar", false, true);
            jobs_rewards.Attributes.Add(jobs_rewards_Pay_rate);
            jobs_rewards.Attributes.Add(jobs_rewards_Time_off);


            Category jobs_risks = new Category(3, "Risks", "risks", "risks");

            Attribute jobs_risks_Occupational_hazards = new Attribute("Occupational_hazards", "Occupational Hazards", "varchar", false, true);
            Attribute jobs_risks_Long_term_risks = new Attribute("Long_term_risks", "Long Term Risks", "varchar", false, true);
            jobs_risks.Attributes.Add(jobs_risks_Occupational_hazards);
            jobs_risks.Attributes.Add(jobs_risks_Long_term_risks);


            Category jobs_specialization = new Category(5, "Specialization", "specialization", "specialization");

            Attribute jobs_specialization_Promotions = new Attribute("Promotions", "Promotions", "varchar", false, true);
            Attribute jobs_specialization_Specializations = new Attribute("Specializations", "Specializations", "tinytext", false, true);
            Attribute jobs_specialization_Field = new Attribute("Field", "Field", "tinytext", false, true);
            Attribute jobs_specialization_Ranks = new Attribute("Ranks", "Ranks", "tinytext", false, true);
            Attribute jobs_specialization_Similar_jobs = new Attribute("Similar_jobs", "Similar Jobs", "varchar", false, true);
            jobs_specialization.Attributes.Add(jobs_specialization_Promotions);
            jobs_specialization.Attributes.Add(jobs_specialization_Specializations);
            jobs_specialization.Attributes.Add(jobs_specialization_Field);
            jobs_specialization.Attributes.Add(jobs_specialization_Ranks);
            jobs_specialization.Attributes.Add(jobs_specialization_Similar_jobs);


            content_jobs.categories.Add(jobs_gallery);
            content_jobs.categories.Add(jobs_history);
            content_jobs.categories.Add(jobs_notes);
            content_jobs.categories.Add(jobs_overview);
            content_jobs.categories.Add(jobs_requirements);
            content_jobs.categories.Add(jobs_rewards);
            content_jobs.categories.Add(jobs_risks);
            content_jobs.categories.Add(jobs_specialization);


            Content content_landmarks = new Content("landmarks", true);

            Category landmarks_appearance = new Category(3, "Eppearance", "appearance", "appearance");



            Category landmarks_ecosystem = new Category(1, "Ecosystem", "ecosystem", "ecosystem");

            Attribute landmarks_ecosystem_Flora = new Attribute("Flora", "Flora", "varchar", false, true);
            Attribute landmarks_ecosystem_Creatures = new Attribute("Creatures", "Creatures", "varchar", false, true);
            landmarks_ecosystem.Attributes.Add(landmarks_ecosystem_Flora);
            landmarks_ecosystem.Attributes.Add(landmarks_ecosystem_Creatures);


            Category landmarks_gallery = new Category(1, "Gallery", "gallery", "gallery");



            Category landmarks_history = new Category(1, "History", "history", "history");

            Attribute landmarks_history_Creation_story = new Attribute("Creation_story", "Creation Story", "tinytext", false, true);
            Attribute landmarks_history_Established_year = new Attribute("Established_year", "Established Year", "int", false, true);
            landmarks_history.Attributes.Add(landmarks_history_Creation_story);
            landmarks_history.Attributes.Add(landmarks_history_Established_year);


            Category landmarks_location = new Category(2, "Location", "location", "location");

            Attribute landmarks_location_Country = new Attribute("Country", "Country", "varchar", false, true);
            Attribute landmarks_location_Nearby_towns = new Attribute("Nearby_towns", "Nearby Towns", "varchar", false, true);
            landmarks_location.Attributes.Add(landmarks_location_Country);
            landmarks_location.Attributes.Add(landmarks_location_Nearby_towns);


            Category landmarks_notes = new Category(1, "Notes", "notes", "notes");

            Attribute landmarks_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute landmarks_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            landmarks_notes.Attributes.Add(landmarks_notes_Notes);
            landmarks_notes.Attributes.Add(landmarks_notes_Private_Notes);


            Category landmarks_overview = new Category(1, "Overview", "overview", "overview");

            Attribute landmarks_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute landmarks_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute landmarks_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute landmarks_overview_Type_of_landmark = new Attribute("Type_of_landmark", "Type Of Landmark", "varchar", false, true);
            Attribute landmarks_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute landmarks_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            landmarks_overview.Attributes.Add(landmarks_overview_Name);
            landmarks_overview.Attributes.Add(landmarks_overview_Description);
            landmarks_overview.Attributes.Add(landmarks_overview_Other_Names);
            landmarks_overview.Attributes.Add(landmarks_overview_Type_of_landmark);
            landmarks_overview.Attributes.Add(landmarks_overview_Universe);
            landmarks_overview.Attributes.Add(landmarks_overview_Tags);


            content_landmarks.categories.Add(landmarks_appearance);
            content_landmarks.categories.Add(landmarks_ecosystem);
            content_landmarks.categories.Add(landmarks_gallery);
            content_landmarks.categories.Add(landmarks_history);
            content_landmarks.categories.Add(landmarks_location);
            content_landmarks.categories.Add(landmarks_notes);
            content_landmarks.categories.Add(landmarks_overview);


            Content content_languages = new Content("languages", true);

            Category languages_entities = new Category(6, "Entities", "entities", "entities");

            Attribute languages_entities_Numbers = new Attribute("Numbers", "Numbers", "varchar", false, true);
            Attribute languages_entities_Quantifiers = new Attribute("Quantifiers", "Quantifiers", "varchar", false, true);
            Attribute languages_entities_Determiners = new Attribute("Determiners", "Determiners", "varchar", false, true);
            Attribute languages_entities_Pronouns = new Attribute("Pronouns", "Pronouns", "varchar", false, true);
            languages_entities.Attributes.Add(languages_entities_Numbers);
            languages_entities.Attributes.Add(languages_entities_Quantifiers);
            languages_entities.Attributes.Add(languages_entities_Determiners);
            languages_entities.Attributes.Add(languages_entities_Pronouns);


            Category languages_gallery = new Category(7, "Gallery", "gallery", "gallery");



            Category languages_grammar = new Category(4, "Grammar", "grammar", "grammar");

            Attribute languages_grammar_Grammar = new Attribute("Grammar", "Grammar", "varchar", false, true);
            languages_grammar.Attributes.Add(languages_grammar_Grammar);


            Category languages_info = new Category(2, "Info", "info", "info");

            Attribute languages_info_History = new Attribute("History", "History", "tinytext", false, true);
            Attribute languages_info_Typology = new Attribute("Typology", "Typology", "varchar", false, true);
            Attribute languages_info_Dialectical_information = new Attribute("Dialectical_information", "Dialectical Information", "varchar", false, true);
            Attribute languages_info_Register = new Attribute("Register", "Register", "varchar", false, true);
            Attribute languages_info_Gestures = new Attribute("Gestures", "Gestures", "varchar", false, true);
            Attribute languages_info_Evolution = new Attribute("Evolution", "Evolution", "varchar", false, true);
            languages_info.Attributes.Add(languages_info_History);
            languages_info.Attributes.Add(languages_info_Typology);
            languages_info.Attributes.Add(languages_info_Dialectical_information);
            languages_info.Attributes.Add(languages_info_Register);
            languages_info.Attributes.Add(languages_info_Gestures);
            languages_info.Attributes.Add(languages_info_Evolution);


            Category languages_notes = new Category(8, "Notes", "notes", "notes");

            Attribute languages_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute languages_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", false, true);
            languages_notes.Attributes.Add(languages_notes_Notes);
            languages_notes.Attributes.Add(languages_notes_Private_notes);


            Category languages_overview = new Category(1, "Overview", "overview", "overview");

            Attribute languages_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute languages_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute languages_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute languages_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            languages_overview.Attributes.Add(languages_overview_Name);
            languages_overview.Attributes.Add(languages_overview_Other_Names);
            languages_overview.Attributes.Add(languages_overview_Universe);
            languages_overview.Attributes.Add(languages_overview_Tags);


            Category languages_phonology = new Category(3, "Phonology", "phonology", "phonology");

            Attribute languages_phonology_Phonology = new Attribute("Phonology", "Phonology", "varchar", false, true);
            languages_phonology.Attributes.Add(languages_phonology_Phonology);


            Category languages_vocabulary = new Category(5, "Vocabulary", "vocabulary", "vocabulary");

            Attribute languages_vocabulary_Please = new Attribute("Please", "Please", "varchar", false, true);
            Attribute languages_vocabulary_Trade = new Attribute("Trade", "Trade", "varchar", false, true);
            Attribute languages_vocabulary_Family = new Attribute("Family", "Family", "varchar", false, true);
            Attribute languages_vocabulary_Body_parts = new Attribute("Body_parts", "Body Parts", "varchar", false, true);
            Attribute languages_vocabulary_No_words = new Attribute("No_words", "No Words", "varchar", false, true);
            Attribute languages_vocabulary_Yes_words = new Attribute("Yes_words", "Yes Words", "varchar", false, true);
            Attribute languages_vocabulary_Sorry = new Attribute("Sorry", "Sorry", "varchar", false, true);
            Attribute languages_vocabulary_You_are_welcome = new Attribute("You_are_welcome", "You Are Welcome", "varchar", false, true);
            Attribute languages_vocabulary_Thank_you = new Attribute("Thank_you", "Thank You", "varchar", false, true);
            Attribute languages_vocabulary_Goodbyes = new Attribute("Goodbyes", "Goodbyes", "varchar", false, true);
            Attribute languages_vocabulary_Greetings = new Attribute("Greetings", "Greetings", "varchar", false, true);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Please);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Trade);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Family);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Body_parts);
            languages_vocabulary.Attributes.Add(languages_vocabulary_No_words);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Yes_words);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Sorry);
            languages_vocabulary.Attributes.Add(languages_vocabulary_You_are_welcome);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Thank_you);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Goodbyes);
            languages_vocabulary.Attributes.Add(languages_vocabulary_Greetings);


            content_languages.categories.Add(languages_entities);
            content_languages.categories.Add(languages_gallery);
            content_languages.categories.Add(languages_grammar);
            content_languages.categories.Add(languages_info);
            content_languages.categories.Add(languages_notes);
            content_languages.categories.Add(languages_overview);
            content_languages.categories.Add(languages_phonology);
            content_languages.categories.Add(languages_vocabulary);


            Content content_locations = new Content("locations", true);

            Category locations_cities = new Category(3, "Cities", "cities", "cities");

            Attribute locations_cities_Capital_cities = new Attribute("Capital_cities", "Capital Cities", "varchar", false, true);
            Attribute locations_cities_Largest_cities = new Attribute("Largest_cities", "Largest Cities", "varchar", false, true);
            Attribute locations_cities_Notable_cities = new Attribute("Notable_cities", "Notable Cities", "varchar", false, true);
            locations_cities.Attributes.Add(locations_cities_Capital_cities);
            locations_cities.Attributes.Add(locations_cities_Largest_cities);
            locations_cities.Attributes.Add(locations_cities_Notable_cities);


            Category locations_culture = new Category(2, "Culture", "culture", "culture");

            Attribute locations_culture_Language = new Attribute("Language", "Language", "varchar", false, true);
            Attribute locations_culture_Population = new Attribute("Population", "Population", "double", false, true);
            Attribute locations_culture_Currency = new Attribute("Currency", "Currency", "varchar", false, true);
            Attribute locations_culture_Motto = new Attribute("Motto", "Motto", "tinytext", false, true);
            Attribute locations_culture_Laws = new Attribute("Laws", "Laws", "tinytext", false, true);
            Attribute locations_culture_Sports = new Attribute("Sports", "Sports", "varchar", false, true);
            Attribute locations_culture_Leaders = new Attribute("Leaders", "Leaders", "varchar", false, true);
            Attribute locations_culture_Spoken_Languages = new Attribute("Spoken_Languages", "Spoken Languages", "varchar", false, true);
            locations_culture.Attributes.Add(locations_culture_Language);
            locations_culture.Attributes.Add(locations_culture_Population);
            locations_culture.Attributes.Add(locations_culture_Currency);
            locations_culture.Attributes.Add(locations_culture_Motto);
            locations_culture.Attributes.Add(locations_culture_Laws);
            locations_culture.Attributes.Add(locations_culture_Sports);
            locations_culture.Attributes.Add(locations_culture_Leaders);
            locations_culture.Attributes.Add(locations_culture_Spoken_Languages);


            Category locations_gallery = new Category(6, "Gallery", "gallery", "gallery");



            Category locations_geography = new Category(4, "Geography", "geography", "geography");

            Attribute locations_geography_Area = new Attribute("Area", "Area", "double", false, true);
            Attribute locations_geography_Crops = new Attribute("Crops", "Crops", "varchar", false, true);
            Attribute locations_geography_Located_at = new Attribute("Located_at", "Located At", "varchar", false, true);
            Attribute locations_geography_Climate = new Attribute("Climate", "Climate", "tinytext", false, true);
            Attribute locations_geography_Landmarks = new Attribute("Landmarks", "Landmarks", "varchar", false, true);
            locations_geography.Attributes.Add(locations_geography_Area);
            locations_geography.Attributes.Add(locations_geography_Crops);
            locations_geography.Attributes.Add(locations_geography_Located_at);
            locations_geography.Attributes.Add(locations_geography_Climate);
            locations_geography.Attributes.Add(locations_geography_Landmarks);


            Category locations_history = new Category(5, "History", "history", "history");

            Attribute locations_history_Notable_Wars = new Attribute("Notable_Wars", "Notable Wars", "tinytext", false, true);
            Attribute locations_history_Founding_Story = new Attribute("Founding_Story", "Founding Story", "tinytext", false, true);
            Attribute locations_history_Established_Year = new Attribute("Established_Year", "Established Year", "int", false, true);
            locations_history.Attributes.Add(locations_history_Notable_Wars);
            locations_history.Attributes.Add(locations_history_Founding_Story);
            locations_history.Attributes.Add(locations_history_Established_Year);


            Category locations_notes = new Category(7, "Notes", "notes", "notes");

            Attribute locations_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute locations_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            locations_notes.Attributes.Add(locations_notes_Notes);
            locations_notes.Attributes.Add(locations_notes_Private_Notes);


            Category locations_overview = new Category(1, "Overview", "overview", "overview");

            Attribute locations_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute locations_overview_Type = new Attribute("Type", "Type", "varchar", false, true);
            Attribute locations_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute locations_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute locations_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            locations_overview.Attributes.Add(locations_overview_Name);
            locations_overview.Attributes.Add(locations_overview_Type);
            locations_overview.Attributes.Add(locations_overview_Description);
            locations_overview.Attributes.Add(locations_overview_Universe);
            locations_overview.Attributes.Add(locations_overview_Tags);


            content_locations.categories.Add(locations_cities);
            content_locations.categories.Add(locations_culture);
            content_locations.categories.Add(locations_gallery);
            content_locations.categories.Add(locations_geography);
            content_locations.categories.Add(locations_history);
            content_locations.categories.Add(locations_notes);
            content_locations.categories.Add(locations_overview);


            Content content_lores = new Content("lores", true);

            Category lores_about = new Category(4, "About", "about", "about");

            Attribute lores_about_Subjects = new Attribute("Subjects", "Subjects", "varchar", false, true);
            Attribute lores_about_Characters = new Attribute("Characters", "Characters", "varchar", false, true);
            Attribute lores_about_Deities = new Attribute("Deities", "Deities", "varchar", false, true);
            Attribute lores_about_Creatures = new Attribute("Creatures", "Creatures", "varchar", false, true);
            Attribute lores_about_Floras = new Attribute("Floras", "Floras", "varchar", false, true);
            Attribute lores_about_Jobs = new Attribute("Jobs", "Jobs", "varchar", false, true);
            Attribute lores_about_Technologies = new Attribute("Technologies", "Technologies", "varchar", false, true);
            Attribute lores_about_Vehicles = new Attribute("Vehicles", "Vehicles", "varchar", false, true);
            Attribute lores_about_Conditions = new Attribute("Conditions", "Conditions", "varchar", false, true);
            Attribute lores_about_Races = new Attribute("Races", "Races", "varchar", false, true);
            Attribute lores_about_Religions = new Attribute("Religions", "Religions", "varchar", false, true);
            Attribute lores_about_Magic = new Attribute("Magic", "Magic", "varchar", false, true);
            Attribute lores_about_Governments = new Attribute("Governments", "Governments", "varchar", false, true);
            Attribute lores_about_Groups = new Attribute("Groups", "Groups", "varchar", false, true);
            Attribute lores_about_Traditions = new Attribute("Traditions", "Traditions", "varchar", false, true);
            Attribute lores_about_Foods = new Attribute("Foods", "Foods", "varchar", false, true);
            Attribute lores_about_Sports = new Attribute("Sports", "Sports", "varchar", false, true);
            lores_about.Attributes.Add(lores_about_Subjects);
            lores_about.Attributes.Add(lores_about_Characters);
            lores_about.Attributes.Add(lores_about_Deities);
            lores_about.Attributes.Add(lores_about_Creatures);
            lores_about.Attributes.Add(lores_about_Floras);
            lores_about.Attributes.Add(lores_about_Jobs);
            lores_about.Attributes.Add(lores_about_Technologies);
            lores_about.Attributes.Add(lores_about_Vehicles);
            lores_about.Attributes.Add(lores_about_Conditions);
            lores_about.Attributes.Add(lores_about_Races);
            lores_about.Attributes.Add(lores_about_Religions);
            lores_about.Attributes.Add(lores_about_Magic);
            lores_about.Attributes.Add(lores_about_Governments);
            lores_about.Attributes.Add(lores_about_Groups);
            lores_about.Attributes.Add(lores_about_Traditions);
            lores_about.Attributes.Add(lores_about_Foods);
            lores_about.Attributes.Add(lores_about_Sports);


            Category lores_content = new Category(2, "Content", "content", "content");

            Attribute lores_content_Full_text = new Attribute("Full_text", "Full Text", "text", false, true);
            Attribute lores_content_Dialect = new Attribute("Dialect", "Dialect", "varchar", false, true);
            Attribute lores_content_Structure = new Attribute("Structure", "Structure", "varchar", false, true);
            Attribute lores_content_Tone = new Attribute("Tone", "Tone", "varchar", false, true);
            Attribute lores_content_Genre = new Attribute("Genre", "Genre", "varchar", false, true);
            lores_content.Attributes.Add(lores_content_Full_text);
            lores_content.Attributes.Add(lores_content_Dialect);
            lores_content.Attributes.Add(lores_content_Structure);
            lores_content.Attributes.Add(lores_content_Tone);
            lores_content.Attributes.Add(lores_content_Genre);


            Category lores_culture = new Category(6, "Culture", "culture", "culture");

            Attribute lores_culture_Impact = new Attribute("Impact", "Impact", "tinytext", false, true);
            Attribute lores_culture_Created_traditions = new Attribute("Created_traditions", "Created Traditions", "tinytext", false, true);
            Attribute lores_culture_Influence_on_modern_times = new Attribute("Influence_on_modern_times", "Influence On Modern Times", "tinytext", false, true);
            Attribute lores_culture_Motivations = new Attribute("Motivations", "Motivations", "varchar", false, true);
            Attribute lores_culture_Reception = new Attribute("Reception", "Reception", "tinytext", false, true);
            Attribute lores_culture_Interpretations = new Attribute("Interpretations", "Interpretations", "tinytext", false, true);
            Attribute lores_culture_Media_adaptations = new Attribute("Media_adaptations", "Media Adaptations", "tinytext", false, true);
            Attribute lores_culture_Criticism = new Attribute("Criticism", "Criticism", "tinytext", false, true);
            Attribute lores_culture_Created_phrases = new Attribute("Created_phrases", "Created Phrases", "tinytext", false, true);
            lores_culture.Attributes.Add(lores_culture_Impact);
            lores_culture.Attributes.Add(lores_culture_Created_traditions);
            lores_culture.Attributes.Add(lores_culture_Influence_on_modern_times);
            lores_culture.Attributes.Add(lores_culture_Motivations);
            lores_culture.Attributes.Add(lores_culture_Reception);
            lores_culture.Attributes.Add(lores_culture_Interpretations);
            lores_culture.Attributes.Add(lores_culture_Media_adaptations);
            lores_culture.Attributes.Add(lores_culture_Criticism);
            lores_culture.Attributes.Add(lores_culture_Created_phrases);


            Category lores_gallery = new Category(10, "Gallery", "gallery", "gallery");



            Category lores_history = new Category(8, "History", "history", "history");

            Attribute lores_history_Historical_context = new Attribute("Historical_context", "Historical Context", "text", false, true);
            Attribute lores_history_Background_information = new Attribute("Background_information", "Background Information", "tinytext", false, true);
            Attribute lores_history_Important_translations = new Attribute("Important_translations", "Important Translations", "tinytext", false, true);
            Attribute lores_history_Propagation_method = new Attribute("Propagation_method", "Propagation Method", "tinytext", false, true);
            lores_history.Attributes.Add(lores_history_Historical_context);
            lores_history.Attributes.Add(lores_history_Background_information);
            lores_history.Attributes.Add(lores_history_Important_translations);
            lores_history.Attributes.Add(lores_history_Propagation_method);


            Category lores_notes = new Category(11, "Notes", "notes", "notes");

            Attribute lores_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute lores_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            lores_notes.Attributes.Add(lores_notes_Notes);
            lores_notes.Attributes.Add(lores_notes_Private_Notes);


            Category lores_origin = new Category(7, "Origin", "origin", "origin");

            Attribute lores_origin_Source = new Attribute("Source", "Source", "tinytext", false, true);
            Attribute lores_origin_Original_telling = new Attribute("Original_telling", "Original Telling", "tinytext", false, true);
            Attribute lores_origin_Date_recorded = new Attribute("Date_recorded", "Date Recorded", "varchar", false, true);
            Attribute lores_origin_Inspirations = new Attribute("Inspirations", "Inspirations", "tinytext", false, true);
            Attribute lores_origin_Original_author = new Attribute("Original_author", "Original Author", "varchar", false, true);
            Attribute lores_origin_Original_languages = new Attribute("Original_languages", "Original Languages", "varchar", false, true);
            lores_origin.Attributes.Add(lores_origin_Source);
            lores_origin.Attributes.Add(lores_origin_Original_telling);
            lores_origin.Attributes.Add(lores_origin_Date_recorded);
            lores_origin.Attributes.Add(lores_origin_Inspirations);
            lores_origin.Attributes.Add(lores_origin_Original_author);
            lores_origin.Attributes.Add(lores_origin_Original_languages);


            Category lores_overview = new Category(1, "Overview", "overview", "overview");

            Attribute lores_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute lores_overview_Summary = new Attribute("Summary", "Summary", "text", false, true);
            Attribute lores_overview_Type = new Attribute("Type", "Type", "varchar", false, true);
            Attribute lores_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute lores_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            lores_overview.Attributes.Add(lores_overview_Name);
            lores_overview.Attributes.Add(lores_overview_Summary);
            lores_overview.Attributes.Add(lores_overview_Type);
            lores_overview.Attributes.Add(lores_overview_Universe);
            lores_overview.Attributes.Add(lores_overview_Tags);


            Category lores_setting = new Category(3, "Setting", "setting", "setting");

            Attribute lores_setting_Time_period = new Attribute("Time_period", "Time Period", "varchar", false, true);
            Attribute lores_setting_Planets = new Attribute("Planets", "Planets", "varchar", false, true);
            Attribute lores_setting_Continents = new Attribute("Continents", "Continents", "varchar", false, true);
            Attribute lores_setting_Countries = new Attribute("Countries", "Countries", "varchar", false, true);
            Attribute lores_setting_Landmarks = new Attribute("Landmarks", "Landmarks", "varchar", false, true);
            Attribute lores_setting_Towns = new Attribute("Towns", "Towns", "varchar", false, true);
            Attribute lores_setting_Buildings = new Attribute("Buildings", "Buildings", "varchar", false, true);
            Attribute lores_setting_Schools = new Attribute("Schools", "Schools", "varchar", false, true);
            lores_setting.Attributes.Add(lores_setting_Time_period);
            lores_setting.Attributes.Add(lores_setting_Planets);
            lores_setting.Attributes.Add(lores_setting_Continents);
            lores_setting.Attributes.Add(lores_setting_Countries);
            lores_setting.Attributes.Add(lores_setting_Landmarks);
            lores_setting.Attributes.Add(lores_setting_Towns);
            lores_setting.Attributes.Add(lores_setting_Buildings);
            lores_setting.Attributes.Add(lores_setting_Schools);


            Category lores_truthiness = new Category(5, "Truthiness", "truthiness", "truthiness");

            Attribute lores_truthiness_True_parts = new Attribute("True_parts", "True Parts", "varchar", false, true);
            Attribute lores_truthiness_false_parts = new Attribute("false_parts", "false Parts", "varchar", false, true);
            Attribute lores_truthiness_Believability = new Attribute("Believability", "Believability", "varchar", false, true);
            Attribute lores_truthiness_Morals = new Attribute("Morals", "Morals", "varchar", false, true);
            Attribute lores_truthiness_Symbolisms = new Attribute("Symbolisms", "Symbolisms", "varchar", false, true);
            Attribute lores_truthiness_Believers = new Attribute("Believers", "Believers", "varchar", false, true);
            Attribute lores_truthiness_Hoaxes = new Attribute("Hoaxes", "Hoaxes", "varchar", false, true);
            lores_truthiness.Attributes.Add(lores_truthiness_True_parts);
            lores_truthiness.Attributes.Add(lores_truthiness_false_parts);
            lores_truthiness.Attributes.Add(lores_truthiness_Believability);
            lores_truthiness.Attributes.Add(lores_truthiness_Morals);
            lores_truthiness.Attributes.Add(lores_truthiness_Symbolisms);
            lores_truthiness.Attributes.Add(lores_truthiness_Believers);
            lores_truthiness.Attributes.Add(lores_truthiness_Hoaxes);


            Category lores_variations = new Category(9, "Variations", "variations", "variations");

            Attribute lores_variations_Geographical_variations = new Attribute("Geographical_variations", "Geographical Variations", "text", false, true);
            Attribute lores_variations_Evolution_over_time = new Attribute("Evolution_over_time", "Evolution Over Time", "tinytext", false, true);
            Attribute lores_variations_Translation_variations = new Attribute("Translation_variations", "Translation Variations", "tinytext", false, true);
            Attribute lores_variations_Variations = new Attribute("Variations", "Variations", "tinytext", false, true);
            Attribute lores_variations_Related_lores = new Attribute("Related_lores", "Related Lores", "tinytext", false, true);
            lores_variations.Attributes.Add(lores_variations_Geographical_variations);
            lores_variations.Attributes.Add(lores_variations_Evolution_over_time);
            lores_variations.Attributes.Add(lores_variations_Translation_variations);
            lores_variations.Attributes.Add(lores_variations_Variations);
            lores_variations.Attributes.Add(lores_variations_Related_lores);


            content_lores.categories.Add(lores_about);
            content_lores.categories.Add(lores_content);
            content_lores.categories.Add(lores_culture);
            content_lores.categories.Add(lores_gallery);
            content_lores.categories.Add(lores_history);
            content_lores.categories.Add(lores_notes);
            content_lores.categories.Add(lores_origin);
            content_lores.categories.Add(lores_overview);
            content_lores.categories.Add(lores_setting);
            content_lores.categories.Add(lores_truthiness);
            content_lores.categories.Add(lores_variations);


            Content content_magics = new Content("magics", true);

            Category magics_alignment = new Category(4, "Alignment", "alignment", "alignment");

            Attribute magics_alignment_Element = new Attribute("Element", "Element", "tinytext", false, true);
            Attribute magics_alignment_Deities = new Attribute("Deities", "Deities", "tinytext", false, true);
            magics_alignment.Attributes.Add(magics_alignment_Element);
            magics_alignment.Attributes.Add(magics_alignment_Deities);


            Category magics_appearance = new Category(2, "Appearance", "appearance", "appearance");

            Attribute magics_appearance_Effects = new Attribute("Effects", "Effects", "tinytext", false, true);
            Attribute magics_appearance_Visuals = new Attribute("Visuals", "Visuals", "tinytext", false, true);
            Attribute magics_appearance_Aftereffects = new Attribute("Aftereffects", "Aftereffects", "tinytext", false, true);
            magics_appearance.Attributes.Add(magics_appearance_Effects);
            magics_appearance.Attributes.Add(magics_appearance_Visuals);
            magics_appearance.Attributes.Add(magics_appearance_Aftereffects);


            Category magics_effects = new Category(3, "Effects", "effects", "effects");

            Attribute magics_effects_Positive_effects = new Attribute("Positive_effects", "Positive Effects", "tinytext", false, true);
            Attribute magics_effects_Neutral_effects = new Attribute("Neutral_effects", "Neutral Effects", "tinytext", false, true);
            Attribute magics_effects_Negative_effects = new Attribute("Negative_effects", "Negative Effects", "tinytext", false, true);
            Attribute magics_effects_Conditions = new Attribute("Conditions", "Conditions", "tinytext", false, true);
            Attribute magics_effects_Scale = new Attribute("Scale", "Scale", "double", false, true);
            magics_effects.Attributes.Add(magics_effects_Positive_effects);
            magics_effects.Attributes.Add(magics_effects_Neutral_effects);
            magics_effects.Attributes.Add(magics_effects_Negative_effects);
            magics_effects.Attributes.Add(magics_effects_Conditions);
            magics_effects.Attributes.Add(magics_effects_Scale);


            Category magics_gallery = new Category(6, "Gallery", "gallery", "gallery");



            Category magics_notes = new Category(7, "Notes", "notes", "notes");

            Attribute magics_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute magics_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", false, true);
            magics_notes.Attributes.Add(magics_notes_Notes);
            magics_notes.Attributes.Add(magics_notes_Private_notes);


            Category magics_overview = new Category(1, "Overview", "overview", "overview");

            Attribute magics_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute magics_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute magics_overview_Type_of_magic = new Attribute("Type_of_magic", "Type Of Magic", "tinytext", false, true);
            Attribute magics_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute magics_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            magics_overview.Attributes.Add(magics_overview_Name);
            magics_overview.Attributes.Add(magics_overview_Description);
            magics_overview.Attributes.Add(magics_overview_Type_of_magic);
            magics_overview.Attributes.Add(magics_overview_Universe);
            magics_overview.Attributes.Add(magics_overview_Tags);


            Category magics_requirements = new Category(5, "Requirements", "requirements", "requirements");

            Attribute magics_requirements_Resource_costs = new Attribute("Resource_costs", "Resource Costs", "tinytext", false, true);
            Attribute magics_requirements_Materials_required = new Attribute("Materials_required", "Materials Required", "tinytext", false, true);
            Attribute magics_requirements_Skills_required = new Attribute("Skills_required", "Skills Required", "tinytext", false, true);
            Attribute magics_requirements_Limitations = new Attribute("Limitations", "Limitations", "tinytext", false, true);
            Attribute magics_requirements_Education = new Attribute("Education", "Education", "tinytext", false, true);
            magics_requirements.Attributes.Add(magics_requirements_Resource_costs);
            magics_requirements.Attributes.Add(magics_requirements_Materials_required);
            magics_requirements.Attributes.Add(magics_requirements_Skills_required);
            magics_requirements.Attributes.Add(magics_requirements_Limitations);
            magics_requirements.Attributes.Add(magics_requirements_Education);


            content_magics.categories.Add(magics_alignment);
            content_magics.categories.Add(magics_appearance);
            content_magics.categories.Add(magics_effects);
            content_magics.categories.Add(magics_gallery);
            content_magics.categories.Add(magics_notes);
            content_magics.categories.Add(magics_overview);
            content_magics.categories.Add(magics_requirements);


            Content content_planets = new Content("planets", true);

            Category planets_astral = new Category(6, "Astral", "astral", "astral");

            Attribute planets_astral_Suns = new Attribute("Suns", "Suns", "varchar", false, true);
            Attribute planets_astral_Moons = new Attribute("Moons", "Moons", "varchar", false, true);
            Attribute planets_astral_Orbit = new Attribute("Orbit", "Orbit", "tinytext", false, true);
            Attribute planets_astral_Visible_Constellations = new Attribute("Visible_Constellations", "Visible Constellations", "tinytext", false, true);
            Attribute planets_astral_Nearby_planets = new Attribute("Nearby_planets", "Nearby Planets", "varchar", false, true);
            planets_astral.Attributes.Add(planets_astral_Suns);
            planets_astral.Attributes.Add(planets_astral_Moons);
            planets_astral.Attributes.Add(planets_astral_Orbit);
            planets_astral.Attributes.Add(planets_astral_Visible_Constellations);
            planets_astral.Attributes.Add(planets_astral_Nearby_planets);


            Category planets_climate = new Category(3, "Climate", "climate", "climate");

            Attribute planets_climate_Seasons = new Attribute("Seasons", "Seasons", "tinytext", false, true);
            Attribute planets_climate_Temperature = new Attribute("Temperature", "Temperature", "double", false, true);
            Attribute planets_climate_Atmosphere = new Attribute("Atmosphere", "Atmosphere", "tinytext", false, true);
            Attribute planets_climate_Natural_diasters = new Attribute("Natural_diasters", "Natural Diasters", "tinytext", false, true);
            planets_climate.Attributes.Add(planets_climate_Seasons);
            planets_climate.Attributes.Add(planets_climate_Temperature);
            planets_climate.Attributes.Add(planets_climate_Atmosphere);
            planets_climate.Attributes.Add(planets_climate_Natural_diasters);


            Category planets_gallery = new Category(8, "Gallery", "gallery", "gallery");



            Category planets_geography = new Category(2, "Geography", "geography", "geography");

            Attribute planets_geography_Size = new Attribute("Size", "Size", "double", false, true);
            Attribute planets_geography_Surface = new Attribute("Surface", "Surface", "double", false, true);
            Attribute planets_geography_Climate = new Attribute("Climate", "Climate", "tinytext", false, true);
            Attribute planets_geography_Calendar_System = new Attribute("Calendar_System", "Calendar System", "tinytext", false, true);
            Attribute planets_geography_Weather = new Attribute("Weather", "Weather", "tinytext", false, true);
            Attribute planets_geography_Water_Content = new Attribute("Water_Content", "Water Content", "double", false, true);
            Attribute planets_geography_Natural_Resources = new Attribute("Natural_Resources", "Natural Resources", "tinytext", false, true);
            Attribute planets_geography_Continents = new Attribute("Continents", "Continents", "varchar", false, true);
            Attribute planets_geography_Countries = new Attribute("Countries", "Countries", "tinytext", false, true);
            Attribute planets_geography_Locations = new Attribute("Locations", "Locations", "tinytext", false, true);
            Attribute planets_geography_Landmarks = new Attribute("Landmarks", "Landmarks", "varchar", false, true);
            planets_geography.Attributes.Add(planets_geography_Size);
            planets_geography.Attributes.Add(planets_geography_Surface);
            planets_geography.Attributes.Add(planets_geography_Climate);
            planets_geography.Attributes.Add(planets_geography_Calendar_System);
            planets_geography.Attributes.Add(planets_geography_Weather);
            planets_geography.Attributes.Add(planets_geography_Water_Content);
            planets_geography.Attributes.Add(planets_geography_Natural_Resources);
            planets_geography.Attributes.Add(planets_geography_Continents);
            planets_geography.Attributes.Add(planets_geography_Countries);
            planets_geography.Attributes.Add(planets_geography_Locations);
            planets_geography.Attributes.Add(planets_geography_Landmarks);


            Category planets_history = new Category(7, "History", "history", "history");

            Attribute planets_history_First_Inhabitants_Story = new Attribute("First_Inhabitants_Story", "First Inhabitants Story", "text", false, true);
            Attribute planets_history_World_History = new Attribute("World_History", "World History", "text", false, true);
            planets_history.Attributes.Add(planets_history_First_Inhabitants_Story);
            planets_history.Attributes.Add(planets_history_World_History);


            Category planets_inhabitants = new Category(5, "Inhabitants", "inhabitants", "inhabitants");

            Attribute planets_inhabitants_Population = new Attribute("Population", "Population", "double", false, true);
            Attribute planets_inhabitants_Races = new Attribute("Races", "Races", "varchar", false, true);
            Attribute planets_inhabitants_Flora = new Attribute("Flora", "Flora", "varchar", false, true);
            Attribute planets_inhabitants_Creatures = new Attribute("Creatures", "Creatures", "varchar", false, true);
            Attribute planets_inhabitants_Religions = new Attribute("Religions", "Religions", "varchar", false, true);
            Attribute planets_inhabitants_Deities = new Attribute("Deities", "Deities", "varchar", false, true);
            Attribute planets_inhabitants_Groups = new Attribute("Groups", "Groups", "varchar", false, true);
            Attribute planets_inhabitants_Languages = new Attribute("Languages", "Languages", "varchar", false, true);
            Attribute planets_inhabitants_Towns = new Attribute("Towns", "Towns", "tinytext", false, true);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Population);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Races);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Flora);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Creatures);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Religions);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Deities);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Groups);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Languages);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Towns);


            Category planets_notes = new Category(9, "Notes", "notes", "notes");

            Attribute planets_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute planets_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            planets_notes.Attributes.Add(planets_notes_Notes);
            planets_notes.Attributes.Add(planets_notes_Private_Notes);


            Category planets_overview = new Category(1, "Overview", "overview", "overview");

            Attribute planets_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute planets_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute planets_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute planets_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            planets_overview.Attributes.Add(planets_overview_Name);
            planets_overview.Attributes.Add(planets_overview_Description);
            planets_overview.Attributes.Add(planets_overview_Universe);
            planets_overview.Attributes.Add(planets_overview_Tags);


            Category planets_time = new Category(4, "Time", "time", "time");

            Attribute planets_time_Length_Of_Day = new Attribute("Length_Of_Day", "Length Of Day", "double", false, true);
            Attribute planets_time_Length_Of_Night = new Attribute("Length_Of_Night", "Length Of Night", "double", false, true);
            Attribute planets_time_Night_sky = new Attribute("Night_sky", "Night Sky", "tinytext", false, true);
            Attribute planets_time_Day_sky = new Attribute("Day_sky", "Day Sky", "tinytext", false, true);
            planets_time.Attributes.Add(planets_time_Length_Of_Day);
            planets_time.Attributes.Add(planets_time_Length_Of_Night);
            planets_time.Attributes.Add(planets_time_Night_sky);
            planets_time.Attributes.Add(planets_time_Day_sky);


            content_planets.categories.Add(planets_astral);
            content_planets.categories.Add(planets_climate);
            content_planets.categories.Add(planets_gallery);
            content_planets.categories.Add(planets_geography);
            content_planets.categories.Add(planets_history);
            content_planets.categories.Add(planets_inhabitants);
            content_planets.categories.Add(planets_notes);
            content_planets.categories.Add(planets_overview);
            content_planets.categories.Add(planets_time);


            Content content_races = new Content("races", true);

            Category races_culture = new Category(4, "Culture", "culture", "culture");

            Attribute races_culture_Famous_figures = new Attribute("Famous_figures", "Famous Figures", "tinytext", false, true);
            Attribute races_culture_Traditions = new Attribute("Traditions", "Traditions", "tinytext", false, true);
            Attribute races_culture_Beliefs = new Attribute("Beliefs", "Beliefs", "tinytext", false, true);
            Attribute races_culture_Governments = new Attribute("Governments", "Governments", "varchar", false, true);
            Attribute races_culture_Technologies = new Attribute("Technologies", "Technologies", "varchar", false, true);
            Attribute races_culture_Occupations = new Attribute("Occupations", "Occupations", "tinytext", false, true);
            Attribute races_culture_Economics = new Attribute("Economics", "Economics", "tinytext", false, true);
            Attribute races_culture_Favorite_foods = new Attribute("Favorite_foods", "Favorite Foods", "tinytext", false, true);
            races_culture.Attributes.Add(races_culture_Famous_figures);
            races_culture.Attributes.Add(races_culture_Traditions);
            races_culture.Attributes.Add(races_culture_Beliefs);
            races_culture.Attributes.Add(races_culture_Governments);
            races_culture.Attributes.Add(races_culture_Technologies);
            races_culture.Attributes.Add(races_culture_Occupations);
            races_culture.Attributes.Add(races_culture_Economics);
            races_culture.Attributes.Add(races_culture_Favorite_foods);


            Category races_gallery = new Category(6, "Gallery", "gallery", "gallery");



            Category races_history = new Category(5, "History", "history", "history");

            Attribute races_history_Notable_events = new Attribute("Notable_events", "Notable Events", "text", false, true);
            races_history.Attributes.Add(races_history_Notable_events);


            Category races_looks = new Category(2, "Looks", "looks", "looks");

            Attribute races_looks_Body_shape = new Attribute("Body_shape", "Body Shape", "double", false, true);
            Attribute races_looks_Skin_colors = new Attribute("Skin_colors", "Skin Colors", "varchar", false, true);
            Attribute races_looks_General_height = new Attribute("General_height", "General Height", "double", false, true);
            Attribute races_looks_General_weight = new Attribute("General_weight", "General Weight", "double", false, true);
            Attribute races_looks_Notable_features = new Attribute("Notable_features", "Notable Features", "tinytext", false, true);
            Attribute races_looks_Physical_variance = new Attribute("Physical_variance", "Physical Variance", "tinytext", false, true);
            Attribute races_looks_Typical_clothing = new Attribute("Typical_clothing", "Typical Clothing", "tinytext", false, true);
            races_looks.Attributes.Add(races_looks_Body_shape);
            races_looks.Attributes.Add(races_looks_Skin_colors);
            races_looks.Attributes.Add(races_looks_General_height);
            races_looks.Attributes.Add(races_looks_General_weight);
            races_looks.Attributes.Add(races_looks_Notable_features);
            races_looks.Attributes.Add(races_looks_Physical_variance);
            races_looks.Attributes.Add(races_looks_Typical_clothing);


            Category races_notes = new Category(7, "Notes", "notes", "notes");

            Attribute races_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute races_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", false, true);
            races_notes.Attributes.Add(races_notes_Notes);
            races_notes.Attributes.Add(races_notes_Private_notes);


            Category races_overview = new Category(1, "Overview", "overview", "overview");

            Attribute races_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute races_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute races_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute races_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute races_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            races_overview.Attributes.Add(races_overview_Name);
            races_overview.Attributes.Add(races_overview_Description);
            races_overview.Attributes.Add(races_overview_Other_Names);
            races_overview.Attributes.Add(races_overview_Universe);
            races_overview.Attributes.Add(races_overview_Tags);


            Category races_traits = new Category(3, "Traits", "traits", "traits");

            Attribute races_traits_Strengths = new Attribute("Strengths", "Strengths", "tinytext", false, true);
            Attribute races_traits_Weaknesses = new Attribute("Weaknesses", "Weaknesses", "tinytext", false, true);
            Attribute races_traits_Conditions = new Attribute("Conditions", "Conditions", "tinytext", false, true);
            races_traits.Attributes.Add(races_traits_Strengths);
            races_traits.Attributes.Add(races_traits_Weaknesses);
            races_traits.Attributes.Add(races_traits_Conditions);


            content_races.categories.Add(races_culture);
            content_races.categories.Add(races_gallery);
            content_races.categories.Add(races_history);
            content_races.categories.Add(races_looks);
            content_races.categories.Add(races_notes);
            content_races.categories.Add(races_overview);
            content_races.categories.Add(races_traits);


            Content content_religions = new Content("religions", true);

            Category religions_beliefs = new Category(3, "Beliefs", "beliefs", "beliefs");

            Attribute religions_beliefs_Deities = new Attribute("Deities", "Deities", "tinytext", false, true);
            Attribute religions_beliefs_Teachings = new Attribute("Teachings", "Teachings", "tinytext", false, true);
            Attribute religions_beliefs_Prophecies = new Attribute("Prophecies", "Prophecies", "tinytext", false, true);
            Attribute religions_beliefs_Places_of_worship = new Attribute("Places_of_worship", "Places Of Worship", "tinytext", false, true);
            Attribute religions_beliefs_Vision_of_paradise = new Attribute("Vision_of_paradise", "Vision Of Paradise", "tinytext", false, true);
            Attribute religions_beliefs_Obligations = new Attribute("Obligations", "Obligations", "tinytext", false, true);
            Attribute religions_beliefs_Worship_services = new Attribute("Worship_services", "Worship Services", "tinytext", false, true);
            religions_beliefs.Attributes.Add(religions_beliefs_Deities);
            religions_beliefs.Attributes.Add(religions_beliefs_Teachings);
            religions_beliefs.Attributes.Add(religions_beliefs_Prophecies);
            religions_beliefs.Attributes.Add(religions_beliefs_Places_of_worship);
            religions_beliefs.Attributes.Add(religions_beliefs_Vision_of_paradise);
            religions_beliefs.Attributes.Add(religions_beliefs_Obligations);
            religions_beliefs.Attributes.Add(religions_beliefs_Worship_services);


            Category religions_gallery = new Category(6, "Gallery", "gallery", "gallery");



            Category religions_history = new Category(2, "History", "history", "history");

            Attribute religions_history_Origin_story = new Attribute("Origin_story", "Origin Story", "text", false, true);
            Attribute religions_history_Notable_figures = new Attribute("Notable_figures", "Notable Figures", "tinytext", false, true);
            Attribute religions_history_Artifacts = new Attribute("Artifacts", "Artifacts", "tinytext", false, true);
            religions_history.Attributes.Add(religions_history_Origin_story);
            religions_history.Attributes.Add(religions_history_Notable_figures);
            religions_history.Attributes.Add(religions_history_Artifacts);


            Category religions_notes = new Category(7, "Notes", "notes", "notes");

            Attribute religions_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute religions_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", false, true);
            religions_notes.Attributes.Add(religions_notes_Notes);
            religions_notes.Attributes.Add(religions_notes_Private_notes);


            Category religions_overview = new Category(1, "Overview", "overview", "overview");

            Attribute religions_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            Attribute religions_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute religions_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute religions_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute religions_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            religions_overview.Attributes.Add(religions_overview_Tags);
            religions_overview.Attributes.Add(religions_overview_Name);
            religions_overview.Attributes.Add(religions_overview_Description);
            religions_overview.Attributes.Add(religions_overview_Other_Names);
            religions_overview.Attributes.Add(religions_overview_Universe);


            Category religions_spread = new Category(5, "Spread", "spread", "spread");

            Attribute religions_spread_Practicing_locations = new Attribute("Practicing_locations", "Practicing Locations", "tinytext", false, true);
            Attribute religions_spread_Practicing_races = new Attribute("Practicing_races", "Practicing Races", "varchar", false, true);
            religions_spread.Attributes.Add(religions_spread_Practicing_locations);
            religions_spread.Attributes.Add(religions_spread_Practicing_races);


            Category religions_traditions = new Category(4, "Traditions", "traditions", "traditions");

            Attribute religions_traditions_Initiation_process = new Attribute("Initiation_process", "Initiation Process", "tinytext", false, true);
            Attribute religions_traditions_Rituals = new Attribute("Rituals", "Rituals", "tinytext", false, true);
            Attribute religions_traditions_Holidays = new Attribute("Holidays", "Holidays", "tinytext", false, true);
            Attribute religions_traditions_Traditions = new Attribute("Traditions", "Traditions", "varchar", false, true);
            religions_traditions.Attributes.Add(religions_traditions_Initiation_process);
            religions_traditions.Attributes.Add(religions_traditions_Rituals);
            religions_traditions.Attributes.Add(religions_traditions_Holidays);
            religions_traditions.Attributes.Add(religions_traditions_Traditions);


            content_religions.categories.Add(religions_beliefs);
            content_religions.categories.Add(religions_gallery);
            content_religions.categories.Add(religions_history);
            content_religions.categories.Add(religions_notes);
            content_religions.categories.Add(religions_overview);
            content_religions.categories.Add(religions_spread);
            content_religions.categories.Add(religions_traditions);


            Content content_scenes = new Content("scenes", true);

            Category scenes_action = new Category(3, "Action", "action", "action");

            Attribute scenes_action_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute scenes_action_Results = new Attribute("Results", "Results", "text", false, true);
            Attribute scenes_action_What_caused_this = new Attribute("What_caused_this", "What Caused This", "text", false, true);
            scenes_action.Attributes.Add(scenes_action_Description);
            scenes_action.Attributes.Add(scenes_action_Results);
            scenes_action.Attributes.Add(scenes_action_What_caused_this);


            Category scenes_gallery = new Category(4, "Gallery", "gallery", "gallery");



            Category scenes_members = new Category(2, "Members", "members", "members");

            Attribute scenes_members_Characters_in_scene = new Attribute("Characters_in_scene", "Characters In Scene", "tinytext", false, true);
            Attribute scenes_members_Locations_in_scene = new Attribute("Locations_in_scene", "Locations In Scene", "tinytext", false, true);
            Attribute scenes_members_Items_in_scene = new Attribute("Items_in_scene", "Items In Scene", "tinytext", false, true);
            scenes_members.Attributes.Add(scenes_members_Characters_in_scene);
            scenes_members.Attributes.Add(scenes_members_Locations_in_scene);
            scenes_members.Attributes.Add(scenes_members_Items_in_scene);


            Category scenes_notes = new Category(5, "Notes", "notes", "notes");

            Attribute scenes_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute scenes_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", false, true);
            scenes_notes.Attributes.Add(scenes_notes_Notes);
            scenes_notes.Attributes.Add(scenes_notes_Private_notes);


            Category scenes_overview = new Category(1, "Overview", "overview", "overview");

            Attribute scenes_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute scenes_overview_Summary = new Attribute("Summary", "Summary", "text", false, true);
            Attribute scenes_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute scenes_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            scenes_overview.Attributes.Add(scenes_overview_Name);
            scenes_overview.Attributes.Add(scenes_overview_Summary);
            scenes_overview.Attributes.Add(scenes_overview_Universe);
            scenes_overview.Attributes.Add(scenes_overview_Tags);


            content_scenes.categories.Add(scenes_action);
            content_scenes.categories.Add(scenes_gallery);
            content_scenes.categories.Add(scenes_members);
            content_scenes.categories.Add(scenes_notes);
            content_scenes.categories.Add(scenes_overview);


            Content content_sports = new Content("sports", true);

            Category sports_culture = new Category(4, "Culture", "culture", "culture");

            Attribute sports_culture_Uniforms = new Attribute("Uniforms", "Uniforms", "tinytext", false, true);
            Attribute sports_culture_Merchandise = new Attribute("Merchandise", "Merchandise", "tinytext", false, true);
            Attribute sports_culture_Popularity = new Attribute("Popularity", "Popularity", "tinytext", false, true);
            Attribute sports_culture_Players = new Attribute("Players", "Players", "tinytext", false, true);
            Attribute sports_culture_Countries = new Attribute("Countries", "Countries", "tinytext", false, true);
            Attribute sports_culture_Teams = new Attribute("Teams", "Teams", "tinytext", false, true);
            Attribute sports_culture_Traditions = new Attribute("Traditions", "Traditions", "tinytext", false, true);
            sports_culture.Attributes.Add(sports_culture_Uniforms);
            sports_culture.Attributes.Add(sports_culture_Merchandise);
            sports_culture.Attributes.Add(sports_culture_Popularity);
            sports_culture.Attributes.Add(sports_culture_Players);
            sports_culture.Attributes.Add(sports_culture_Countries);
            sports_culture.Attributes.Add(sports_culture_Teams);
            sports_culture.Attributes.Add(sports_culture_Traditions);


            Category sports_gallery = new Category(6, "Gallery", "gallery", "gallery");



            Category sports_history = new Category(5, "History", "history", "history");

            Attribute sports_history_Origin_story = new Attribute("Origin_story", "Origin Story", "text", false, true);
            Attribute sports_history_Creators = new Attribute("Creators", "Creators", "tinytext", false, true);
            Attribute sports_history_Evolution = new Attribute("Evolution", "Evolution", "tinytext", false, true);
            Attribute sports_history_Famous_games = new Attribute("Famous_games", "Famous Games", "tinytext", false, true);
            sports_history.Attributes.Add(sports_history_Origin_story);
            sports_history.Attributes.Add(sports_history_Creators);
            sports_history.Attributes.Add(sports_history_Evolution);
            sports_history.Attributes.Add(sports_history_Famous_games);


            Category sports_notes = new Category(7, "Notes", "notes", "notes");

            Attribute sports_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute sports_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            sports_notes.Attributes.Add(sports_notes_Notes);
            sports_notes.Attributes.Add(sports_notes_Private_Notes);


            Category sports_overview = new Category(1, "Overview", "overview", "overview");

            Attribute sports_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute sports_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute sports_overview_Nicknames = new Attribute("Nicknames", "Nicknames", "text", false, true);
            Attribute sports_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute sports_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            sports_overview.Attributes.Add(sports_overview_Name);
            sports_overview.Attributes.Add(sports_overview_Description);
            sports_overview.Attributes.Add(sports_overview_Nicknames);
            sports_overview.Attributes.Add(sports_overview_Universe);
            sports_overview.Attributes.Add(sports_overview_Tags);


            Category sports_playing = new Category(3, "Playing", "playing", "playing");

            Attribute sports_playing_Rules = new Attribute("Rules", "Rules", "tinytext", false, true);
            Attribute sports_playing_Game_time = new Attribute("Game_time", "Game Time", "double", false, true);
            Attribute sports_playing_Positions = new Attribute("Positions", "Positions", "int", false, true);
            Attribute sports_playing_Strategies = new Attribute("Strategies", "Strategies", "varchar", false, true);
            Attribute sports_playing_Common_injuries = new Attribute("Common_injuries", "Common Injuries", "tinytext", false, true);
            Attribute sports_playing_Most_important_muscles = new Attribute("Most_important_muscles", "Most Important Muscles", "varchar", false, true);
            sports_playing.Attributes.Add(sports_playing_Rules);
            sports_playing.Attributes.Add(sports_playing_Game_time);
            sports_playing.Attributes.Add(sports_playing_Positions);
            sports_playing.Attributes.Add(sports_playing_Strategies);
            sports_playing.Attributes.Add(sports_playing_Common_injuries);
            sports_playing.Attributes.Add(sports_playing_Most_important_muscles);


            Category sports_setup = new Category(2, "Setup", "setup", "setup");

            Attribute sports_setup_Play_area = new Attribute("Play_area", "Play Area", "varchar", false, true);
            Attribute sports_setup_Equipment = new Attribute("Equipment", "Equipment", "varchar", false, true);
            Attribute sports_setup_Number_of_players = new Attribute("Number_of_players", "Number Of Players", "int", false, true);
            Attribute sports_setup_Scoring = new Attribute("Scoring", "Scoring", "varchar", false, true);
            Attribute sports_setup_Penalties = new Attribute("Penalties", "Penalties", "varchar", false, true);
            Attribute sports_setup_How_to_win = new Attribute("How_to_win", "How To Win", "tinytext", false, true);
            sports_setup.Attributes.Add(sports_setup_Play_area);
            sports_setup.Attributes.Add(sports_setup_Equipment);
            sports_setup.Attributes.Add(sports_setup_Number_of_players);
            sports_setup.Attributes.Add(sports_setup_Scoring);
            sports_setup.Attributes.Add(sports_setup_Penalties);
            sports_setup.Attributes.Add(sports_setup_How_to_win);


            content_sports.categories.Add(sports_culture);
            content_sports.categories.Add(sports_gallery);
            content_sports.categories.Add(sports_history);
            content_sports.categories.Add(sports_notes);
            content_sports.categories.Add(sports_overview);
            content_sports.categories.Add(sports_playing);
            content_sports.categories.Add(sports_setup);


            Content content_technologies = new Content("technologies", true);

            Category technologies_appearance = new Category(5, "Appearance", "appearance", "appearance");

            Attribute technologies_appearance_Physical_Description = new Attribute("Physical_Description", "Physical Description", "tinytext", false, true);
            Attribute technologies_appearance_Size = new Attribute("Size", "Size", "double", false, true);
            Attribute technologies_appearance_Weight = new Attribute("Weight", "Weight", "double", false, true);
            Attribute technologies_appearance_Colors = new Attribute("Colors", "Colors", "varchar", false, true);
            technologies_appearance.Attributes.Add(technologies_appearance_Physical_Description);
            technologies_appearance.Attributes.Add(technologies_appearance_Size);
            technologies_appearance.Attributes.Add(technologies_appearance_Weight);
            technologies_appearance.Attributes.Add(technologies_appearance_Colors);


            Category technologies_gallery = new Category(7, "Gallery", "gallery", "gallery");



            Category technologies_notes = new Category(8, "Notes", "notes", "notes");

            Attribute technologies_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute technologies_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            technologies_notes.Attributes.Add(technologies_notes_Notes);
            technologies_notes.Attributes.Add(technologies_notes_Private_Notes);


            Category technologies_overview = new Category(1, "Overview", "overview", "overview");

            Attribute technologies_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute technologies_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute technologies_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", false, true);
            Attribute technologies_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute technologies_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            technologies_overview.Attributes.Add(technologies_overview_Name);
            technologies_overview.Attributes.Add(technologies_overview_Description);
            technologies_overview.Attributes.Add(technologies_overview_Other_Names);
            technologies_overview.Attributes.Add(technologies_overview_Universe);
            technologies_overview.Attributes.Add(technologies_overview_Tags);


            Category technologies_presence = new Category(3, "Presence", "presence", "presence");

            Attribute technologies_presence_Characters = new Attribute("Characters", "Characters", "tinytext", false, true);
            Attribute technologies_presence_Towns = new Attribute("Towns", "Towns", "varchar", false, true);
            Attribute technologies_presence_Countries = new Attribute("Countries", "Countries", "varchar", false, true);
            Attribute technologies_presence_Groups = new Attribute("Groups", "Groups", "varchar", false, true);
            Attribute technologies_presence_Creatures = new Attribute("Creatures", "Creatures", "varchar", false, true);
            Attribute technologies_presence_Planets = new Attribute("Planets", "Planets", "varchar", false, true);
            Attribute technologies_presence_Rarity = new Attribute("Rarity", "Rarity", "varchar", false, true);
            technologies_presence.Attributes.Add(technologies_presence_Characters);
            technologies_presence.Attributes.Add(technologies_presence_Towns);
            technologies_presence.Attributes.Add(technologies_presence_Countries);
            technologies_presence.Attributes.Add(technologies_presence_Groups);
            technologies_presence.Attributes.Add(technologies_presence_Creatures);
            technologies_presence.Attributes.Add(technologies_presence_Planets);
            technologies_presence.Attributes.Add(technologies_presence_Rarity);


            Category technologies_production = new Category(2, "Production", "production", "production");

            Attribute technologies_production_Materials = new Attribute("Materials", "Materials", "tinytext", false, true);
            Attribute technologies_production_Manufacturing_Process = new Attribute("Manufacturing_Process", "Manufacturing Process", "tinytext", false, true);
            Attribute technologies_production_Sales_Process = new Attribute("Sales_Process", "Sales Process", "varchar", false, true);
            Attribute technologies_production_Cost = new Attribute("Cost", "Cost", "double", false, true);
            technologies_production.Attributes.Add(technologies_production_Materials);
            technologies_production.Attributes.Add(technologies_production_Manufacturing_Process);
            technologies_production.Attributes.Add(technologies_production_Sales_Process);
            technologies_production.Attributes.Add(technologies_production_Cost);


            Category technologies_related_technologies = new Category(6, "Related Technologies", "related_technologies", "related_technologies");

            Attribute technologies_related_technologies_Related_technologies = new Attribute("Related_technologies", "Related Technologies", "varchar", false, true);
            Attribute technologies_related_technologies_Parent_technologies = new Attribute("Parent_technologies", "Parent Technologies", "varchar", false, true);
            Attribute technologies_related_technologies_Child_technologies = new Attribute("Child_technologies", "Child Technologies", "varchar", false, true);
            technologies_related_technologies.Attributes.Add(technologies_related_technologies_Related_technologies);
            technologies_related_technologies.Attributes.Add(technologies_related_technologies_Parent_technologies);
            technologies_related_technologies.Attributes.Add(technologies_related_technologies_Child_technologies);


            Category technologies_use = new Category(4, "Use", "use", "use");

            Attribute technologies_use_Purpose = new Attribute("Purpose", "Purpose", "tinytext", false, true);
            Attribute technologies_use_How_It_Works = new Attribute("How_It_Works", "How It Works", "text", false, true);
            Attribute technologies_use_Resources_Used = new Attribute("Resources_Used", "Resources Used", "tinytext", false, true);
            Attribute technologies_use_Magic_effects = new Attribute("Magic_effects", "Magic Effects", "tinytext", false, true);
            technologies_use.Attributes.Add(technologies_use_Purpose);
            technologies_use.Attributes.Add(technologies_use_How_It_Works);
            technologies_use.Attributes.Add(technologies_use_Resources_Used);
            technologies_use.Attributes.Add(technologies_use_Magic_effects);


            content_technologies.categories.Add(technologies_appearance);
            content_technologies.categories.Add(technologies_gallery);
            content_technologies.categories.Add(technologies_notes);
            content_technologies.categories.Add(technologies_overview);
            content_technologies.categories.Add(technologies_presence);
            content_technologies.categories.Add(technologies_production);
            content_technologies.categories.Add(technologies_related_technologies);
            content_technologies.categories.Add(technologies_use);


            Content content_towns = new Content("towns", true);

            Category towns_culture = new Category(4, "Culture", "culture", "culture");

            Attribute towns_culture_Laws = new Attribute("Laws", "Laws", "text", false, true);
            Attribute towns_culture_Languages = new Attribute("Languages", "Languages", "varchar", false, true);
            Attribute towns_culture_Flora = new Attribute("Flora", "Flora", "varchar", false, true);
            Attribute towns_culture_Creatures = new Attribute("Creatures", "Creatures", "text", false, true);
            Attribute towns_culture_Politics = new Attribute("Politics", "Politics", "text", false, true);
            Attribute towns_culture_Sports = new Attribute("Sports", "Sports", "tinytext", false, true);
            towns_culture.Attributes.Add(towns_culture_Laws);
            towns_culture.Attributes.Add(towns_culture_Languages);
            towns_culture.Attributes.Add(towns_culture_Flora);
            towns_culture.Attributes.Add(towns_culture_Creatures);
            towns_culture.Attributes.Add(towns_culture_Politics);
            towns_culture.Attributes.Add(towns_culture_Sports);


            Category towns_gallery = new Category(7, "Gallery", "gallery", "gallery");



            Category towns_history = new Category(5, "History", "history", "history");

            Attribute towns_history_Founding_story = new Attribute("Founding_story", "Founding Story", "tinytext", false, true);
            Attribute towns_history_Established_year = new Attribute("Established_year", "Established Year", "int", false, true);
            towns_history.Attributes.Add(towns_history_Founding_story);
            towns_history.Attributes.Add(towns_history_Established_year);


            Category towns_layout = new Category(3, "Layout", "layout", "layout");

            Attribute towns_layout_Buildings = new Attribute("Buildings", "Buildings", "int", false, true);
            Attribute towns_layout_Neighborhoods = new Attribute("Neighborhoods", "Neighborhoods", "int", false, true);
            Attribute towns_layout_Busy_areas = new Attribute("Busy_areas", "Busy Areas", "tinytext", false, true);
            Attribute towns_layout_Landmarks = new Attribute("Landmarks", "Landmarks", "tinytext", false, true);
            towns_layout.Attributes.Add(towns_layout_Buildings);
            towns_layout.Attributes.Add(towns_layout_Neighborhoods);
            towns_layout.Attributes.Add(towns_layout_Busy_areas);
            towns_layout.Attributes.Add(towns_layout_Landmarks);


            Category towns_notes = new Category(8, "Notes", "notes", "notes");

            Attribute towns_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute towns_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            towns_notes.Attributes.Add(towns_notes_Notes);
            towns_notes.Attributes.Add(towns_notes_Private_Notes);


            Category towns_overview = new Category(1, "Overview", "overview", "overview");

            Attribute towns_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute towns_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute towns_overview_Other_names = new Attribute("Other_names", "Other Names", "varchar", false, true);
            Attribute towns_overview_Country = new Attribute("Country", "Country", "int", false, true);
            Attribute towns_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute towns_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            towns_overview.Attributes.Add(towns_overview_Name);
            towns_overview.Attributes.Add(towns_overview_Description);
            towns_overview.Attributes.Add(towns_overview_Other_names);
            towns_overview.Attributes.Add(towns_overview_Country);
            towns_overview.Attributes.Add(towns_overview_Universe);
            towns_overview.Attributes.Add(towns_overview_Tags);


            Category towns_populace = new Category(2, "Populace", "populace", "populace");

            Attribute towns_populace_Citizens = new Attribute("Citizens", "Citizens", "double", false, true);
            Attribute towns_populace_Groups = new Attribute("Groups", "Groups", "varchar", false, true);
            towns_populace.Attributes.Add(towns_populace_Citizens);
            towns_populace.Attributes.Add(towns_populace_Groups);


            Category towns_sustainability = new Category(6, "Sustainability", "sustainability", "sustainability");

            Attribute towns_sustainability_Food_sources = new Attribute("Food_sources", "Food Sources", "tinytext", false, true);
            Attribute towns_sustainability_Energy_sources = new Attribute("Energy_sources", "Energy Sources", "tinytext", false, true);
            Attribute towns_sustainability_Recycling = new Attribute("Recycling", "Recycling", "tinytext", false, true);
            Attribute towns_sustainability_Waste = new Attribute("Waste", "Waste", "tinytext", false, true);
            towns_sustainability.Attributes.Add(towns_sustainability_Food_sources);
            towns_sustainability.Attributes.Add(towns_sustainability_Energy_sources);
            towns_sustainability.Attributes.Add(towns_sustainability_Recycling);
            towns_sustainability.Attributes.Add(towns_sustainability_Waste);


            content_towns.categories.Add(towns_culture);
            content_towns.categories.Add(towns_gallery);
            content_towns.categories.Add(towns_history);
            content_towns.categories.Add(towns_layout);
            content_towns.categories.Add(towns_notes);
            content_towns.categories.Add(towns_overview);
            content_towns.categories.Add(towns_populace);
            content_towns.categories.Add(towns_sustainability);


            Content content_traditions = new Content("traditions", true);

            Category traditions_celebrations = new Category(3, "Celebrations", "celebrations", "celebrations");

            Attribute traditions_celebrations_Activities = new Attribute("Activities", "Activities", "varchar", false, true);
            Attribute traditions_celebrations_Gifts = new Attribute("Gifts", "Gifts", "varchar", false, true);
            Attribute traditions_celebrations_Food = new Attribute("Food", "Food", "varchar", false, true);
            Attribute traditions_celebrations_Symbolism = new Attribute("Symbolism", "Symbolism", "varchar", false, true);
            Attribute traditions_celebrations_Games = new Attribute("Games", "Games", "varchar", false, true);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Activities);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Gifts);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Food);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Symbolism);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Games);


            Category traditions_gallery = new Category(5, "Gallery", "gallery", "gallery");



            Category traditions_history = new Category(4, "History", "history", "history");

            Attribute traditions_history_Etymology = new Attribute("Etymology", "Etymology", "varchar", false, true);
            Attribute traditions_history_Origin = new Attribute("Origin", "Origin", "text", false, true);
            Attribute traditions_history_Significance = new Attribute("Significance", "Significance", "varchar", false, true);
            Attribute traditions_history_Religions = new Attribute("Religions", "Religions", "varchar", false, true);
            Attribute traditions_history_Notable_events = new Attribute("Notable_events", "Notable Events", "text", false, true);
            traditions_history.Attributes.Add(traditions_history_Etymology);
            traditions_history.Attributes.Add(traditions_history_Origin);
            traditions_history.Attributes.Add(traditions_history_Significance);
            traditions_history.Attributes.Add(traditions_history_Religions);
            traditions_history.Attributes.Add(traditions_history_Notable_events);


            Category traditions_notes = new Category(6, "Notes", "notes", "notes");

            Attribute traditions_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute traditions_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            traditions_notes.Attributes.Add(traditions_notes_Notes);
            traditions_notes.Attributes.Add(traditions_notes_Private_Notes);


            Category traditions_observance = new Category(2, "Observance", "observance", "observance");

            Attribute traditions_observance_Dates = new Attribute("Dates", "Dates", "text", false, true);
            Attribute traditions_observance_Countries = new Attribute("Countries", "Countries", "varchar", false, true);
            Attribute traditions_observance_Groups = new Attribute("Groups", "Groups", "varchar", false, true);
            Attribute traditions_observance_Towns = new Attribute("Towns", "Towns", "varchar", false, true);
            traditions_observance.Attributes.Add(traditions_observance_Dates);
            traditions_observance.Attributes.Add(traditions_observance_Countries);
            traditions_observance.Attributes.Add(traditions_observance_Groups);
            traditions_observance.Attributes.Add(traditions_observance_Towns);


            Category traditions_overview = new Category(1, "Overview", "overview", "overview");

            Attribute traditions_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute traditions_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute traditions_overview_Type_of_tradition = new Attribute("Type_of_tradition", "Type Of Tradition", "varchar", false, true);
            Attribute traditions_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute traditions_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "tinytext", false, true);
            Attribute traditions_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            traditions_overview.Attributes.Add(traditions_overview_Name);
            traditions_overview.Attributes.Add(traditions_overview_Description);
            traditions_overview.Attributes.Add(traditions_overview_Type_of_tradition);
            traditions_overview.Attributes.Add(traditions_overview_Universe);
            traditions_overview.Attributes.Add(traditions_overview_Alternate_names);
            traditions_overview.Attributes.Add(traditions_overview_Tags);


            content_traditions.categories.Add(traditions_celebrations);
            content_traditions.categories.Add(traditions_gallery);
            content_traditions.categories.Add(traditions_history);
            content_traditions.categories.Add(traditions_notes);
            content_traditions.categories.Add(traditions_observance);
            content_traditions.categories.Add(traditions_overview);


            Content content_universes = new Content("universes", true);

            Category universes_contributors = new Category(6, "Contributors", "contributors", "contributors");



            Category universes_gallery = new Category(5, "Gallery", "gallery", "gallery");



            Category universes_history = new Category(2, "History", "history", "history");

            Attribute universes_history_history = new Attribute("history", "History", "longblob", false, true);
            universes_history.Attributes.Add(universes_history_history);


            Category universes_notes = new Category(4, "Notes", "notes", "notes");

            Attribute universes_notes_notes = new Attribute("notes", "Notes", "longblob", false, true);
            Attribute universes_notes_private_notes = new Attribute("private_notes", "Private Notes", "text", false, true);
            universes_notes.Attributes.Add(universes_notes_notes);
            universes_notes.Attributes.Add(universes_notes_private_notes);


            Category universes_overview = new Category(1, "Overview", "overview", "overview");

            Attribute universes_overview_name = new Attribute("name", "Name", "varchar", false, true);
            Attribute universes_overview_description = new Attribute("description", "Description", "longblob", false, true);
            Attribute universes_overview_genre = new Attribute("genre", "Genre", "varchar", false, true);
            Attribute universes_overview_privacy = new Attribute("privacy", "Privacy", "tinyint", false, true);
            Attribute universes_overview_favorite = new Attribute("favorite", "Favorite", "tinyint", false, true);
            universes_overview.Attributes.Add(universes_overview_name);
            universes_overview.Attributes.Add(universes_overview_description);
            universes_overview.Attributes.Add(universes_overview_genre);
            universes_overview.Attributes.Add(universes_overview_privacy);
            universes_overview.Attributes.Add(universes_overview_favorite);


            Category universes_rules = new Category(3, "Rules", "rules", "rules");

            Attribute universes_rules_laws_of_physics = new Attribute("laws_of_physics", "Laws Of Physics", "text", false, true);
            Attribute universes_rules_magic_system = new Attribute("magic_system", "Magic System", "text", false, true);
            Attribute universes_rules_technology = new Attribute("technology", "Technology", "text", false, true);
            universes_rules.Attributes.Add(universes_rules_laws_of_physics);
            universes_rules.Attributes.Add(universes_rules_magic_system);
            universes_rules.Attributes.Add(universes_rules_technology);


            content_universes.categories.Add(universes_contributors);
            content_universes.categories.Add(universes_gallery);
            content_universes.categories.Add(universes_history);
            content_universes.categories.Add(universes_notes);
            content_universes.categories.Add(universes_overview);
            content_universes.categories.Add(universes_rules);


            Content content_vehicles = new Content("vehicles", true);

            Category vehicles_gallery = new Category(6, "Gallery", "gallery", "gallery");



            Category vehicles_looks = new Category(2, "Looks", "looks", "looks");

            Attribute vehicles_looks_Size = new Attribute("Size", "Size", "double", false, true);
            Attribute vehicles_looks_Doors = new Attribute("Doors", "Doors", "int", false, true);
            Attribute vehicles_looks_Materials = new Attribute("Materials", "Materials", "text", false, true);
            Attribute vehicles_looks_Dimensions = new Attribute("Dimensions", "Dimensions", "double", false, true);
            Attribute vehicles_looks_Windows = new Attribute("Windows", "Windows", "int", false, true);
            Attribute vehicles_looks_Colors = new Attribute("Colors", "Colors", "varchar", false, true);
            Attribute vehicles_looks_Designer = new Attribute("Designer", "Designer", "varchar", false, true);
            vehicles_looks.Attributes.Add(vehicles_looks_Size);
            vehicles_looks.Attributes.Add(vehicles_looks_Doors);
            vehicles_looks.Attributes.Add(vehicles_looks_Materials);
            vehicles_looks.Attributes.Add(vehicles_looks_Dimensions);
            vehicles_looks.Attributes.Add(vehicles_looks_Windows);
            vehicles_looks.Attributes.Add(vehicles_looks_Colors);
            vehicles_looks.Attributes.Add(vehicles_looks_Designer);


            Category vehicles_manufacturing = new Category(4, "Manufacturing", "manufacturing", "manufacturing");

            Attribute vehicles_manufacturing_Manufacturer = new Attribute("Manufacturer", "Manufacturer", "varchar", false, true);
            Attribute vehicles_manufacturing_Costs = new Attribute("Costs", "Costs", "double", false, true);
            Attribute vehicles_manufacturing_Weight = new Attribute("Weight", "Weight", "double", false, true);
            Attribute vehicles_manufacturing_Country = new Attribute("Country", "Country", "int", false, true);
            Attribute vehicles_manufacturing_Variants = new Attribute("Variants", "Variants", "tinytext", false, true);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Manufacturer);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Costs);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Weight);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Country);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Variants);


            Category vehicles_notes = new Category(7, "Notes", "notes", "notes");

            Attribute vehicles_notes_Notes = new Attribute("Notes", "Notes", "text", false, true);
            Attribute vehicles_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", false, true);
            vehicles_notes.Attributes.Add(vehicles_notes_Notes);
            vehicles_notes.Attributes.Add(vehicles_notes_Private_Notes);


            Category vehicles_operators = new Category(5, "Operators", "operators", "operators");

            Attribute vehicles_operators_Owner = new Attribute("Owner", "Owner", "varchar", false, true);
            vehicles_operators.Attributes.Add(vehicles_operators_Owner);


            Category vehicles_overview = new Category(1, "Overview", "overview", "overview");

            Attribute vehicles_overview_Name = new Attribute("Name", "Name", "varchar", false, true);
            Attribute vehicles_overview_Description = new Attribute("Description", "Description", "text", false, true);
            Attribute vehicles_overview_Universe = new Attribute("Universe", "Universe", "bigint", false, true);
            Attribute vehicles_overview_Type_of_vehicle = new Attribute("Type_of_vehicle", "Type Of Vehicle", "text", false, true);
            Attribute vehicles_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "text", false, true);
            Attribute vehicles_overview_Tags = new Attribute("Tags", "Tags", "varchar", false, true);
            vehicles_overview.Attributes.Add(vehicles_overview_Name);
            vehicles_overview.Attributes.Add(vehicles_overview_Description);
            vehicles_overview.Attributes.Add(vehicles_overview_Universe);
            vehicles_overview.Attributes.Add(vehicles_overview_Type_of_vehicle);
            vehicles_overview.Attributes.Add(vehicles_overview_Alternate_names);
            vehicles_overview.Attributes.Add(vehicles_overview_Tags);


            Category vehicles_travel = new Category(3, "Travel", "travel", "travel");

            Attribute vehicles_travel_Speed = new Attribute("Speed", "Speed", "double", false, true);
            Attribute vehicles_travel_Distance = new Attribute("Distance", "Distance", "varchar", false, true);
            Attribute vehicles_travel_Fuel = new Attribute("Fuel", "Fuel", "double", false, true);
            Attribute vehicles_travel_Safety = new Attribute("Safety", "Safety", "text", false, true);
            Attribute vehicles_travel_Features = new Attribute("Features", "Features", "text", false, true);
            vehicles_travel.Attributes.Add(vehicles_travel_Speed);
            vehicles_travel.Attributes.Add(vehicles_travel_Distance);
            vehicles_travel.Attributes.Add(vehicles_travel_Fuel);
            vehicles_travel.Attributes.Add(vehicles_travel_Safety);
            vehicles_travel.Attributes.Add(vehicles_travel_Features);


            content_vehicles.categories.Add(vehicles_gallery);
            content_vehicles.categories.Add(vehicles_looks);
            content_vehicles.categories.Add(vehicles_manufacturing);
            content_vehicles.categories.Add(vehicles_notes);
            content_vehicles.categories.Add(vehicles_operators);
            content_vehicles.categories.Add(vehicles_overview);
            content_vehicles.categories.Add(vehicles_travel);


            contentTemplate.Contents.Add(content_buildings);
            contentTemplate.Contents.Add(content_characters);
            contentTemplate.Contents.Add(content_conditions);
            contentTemplate.Contents.Add(content_continents);
            contentTemplate.Contents.Add(content_countries);
            contentTemplate.Contents.Add(content_creatures);
            contentTemplate.Contents.Add(content_deities);
            contentTemplate.Contents.Add(content_floras);
            contentTemplate.Contents.Add(content_foods);
            contentTemplate.Contents.Add(content_governments);
            contentTemplate.Contents.Add(content_groups);
            contentTemplate.Contents.Add(content_items);
            contentTemplate.Contents.Add(content_jobs);
            contentTemplate.Contents.Add(content_landmarks);
            contentTemplate.Contents.Add(content_languages);
            contentTemplate.Contents.Add(content_locations);
            contentTemplate.Contents.Add(content_lores);
            contentTemplate.Contents.Add(content_magics);
            contentTemplate.Contents.Add(content_planets);
            contentTemplate.Contents.Add(content_races);
            contentTemplate.Contents.Add(content_religions);
            contentTemplate.Contents.Add(content_scenes);
            contentTemplate.Contents.Add(content_sports);
            contentTemplate.Contents.Add(content_technologies);
            contentTemplate.Contents.Add(content_towns);
            contentTemplate.Contents.Add(content_traditions);
            contentTemplate.Contents.Add(content_universes);
            contentTemplate.Contents.Add(content_vehicles);


            #endregion


            var jsonTemplate = JsonConvert.SerializeObject(contentTemplate, Formatting.Indented);
            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            "my_book", "Templates_MOD", "Content_Template"+ DateTime.Now.ToShortDateString()  +".json"), jsonTemplate, true);
        }

        void GenerateCodeFromJSON()
        {

            string filePath = @"C:\Users\sande\Desktop\my_book\Templates_MOD\Content_Template.json";

            ContentTemplate contentTemplate = new ContentTemplate();

            var jsonContent = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(jsonContent))
            {
                contentTemplate = JsonConvert.DeserializeObject<ContentTemplate>(jsonContent);
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("#region CODE");
            StringBuilder sbContentADD = new StringBuilder();

            foreach (var contentType in contentTemplate.Contents)
            {
                sb.AppendLine("Content content_" + contentType.content_type + " = new Content(\"" + contentType.content_type + "\", true);");
                sbContentADD.AppendLine("contentTemplate.Contents.Add(content_" + contentType.content_type + ");");

                StringBuilder sbCatADD = new StringBuilder();

                foreach (var cat in contentType.categories)
                {
                    sb.AppendLine();
                    sb.AppendLine("Category " + contentType.content_type + "_" + cat.Icon + " = new Category(" + cat.Order + ", \"" + cat.Label + "\", \"" + cat.Icon + "\", \"" + cat.Icon + "\");");
                    sbCatADD.AppendLine("content_" + contentType.content_type + ".categories.Add(" + contentType.content_type + "_" + cat.Icon + ");");

                    sb.AppendLine();
                    StringBuilder sbCatAttrADD = new StringBuilder();
                    foreach (var att in cat.Attributes)
                    {
                        sb.AppendLine("Attribute " + contentType.content_type + "_" + cat.Icon + "_" + att.field_name + " = new Attribute(\"" + att.field_name + "\", \"" + att.field_label + "\", \"" + att.field_type + "\", " + att.auto_increament + ", true);");
                        sbCatAttrADD.AppendLine("" + contentType.content_type + "_" + cat.Icon + ".Attributes.Add(" + contentType.content_type + "_" + cat.Icon + "_" + att.field_name + ");");
                    }

                    sb.AppendLine(sbCatAttrADD.ToString());
                }
                sb.AppendLine();
                sb.AppendLine(sbCatADD.ToString());
                sb.AppendLine();
            }
            sb.AppendLine(sbContentADD.ToString());
            sb.AppendLine();
            sb.AppendLine("#endregion");
            //var jsonTemplate = JsonConvert.SerializeObject(contentTemplate, Formatting.Indented);
            //FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            //"my_book", "Templates_MOD", "Content_Template.json"), jsonTemplate, true);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            "my_book", "Content_Template.cs"), sb.ToString(), true);

        }
    }
}
