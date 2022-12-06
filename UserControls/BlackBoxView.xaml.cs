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
using ToDoStuff.Helpers;
using ToDoStuff.Model;
using ToDoStuff.Utility;
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
            txtFilePath.Text = @"E:\AppDumps\ScrapOut\manytoon.txt";
            txtPostfix.Text = "/";
        }

        public string DisplayName { get; set; }

        public UserControl LoadControl()
        {
            return this;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex.Message);
            }

            MessageBox.Show("Complete");
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //var maxFileSize = Bytes(Convert.ToDouble(txtBasePath.Text));
            //MessageBox.Show(Convert.ToString(maxFileSize));

            //    MinoFileUpload minoFileUpload = new MinoFileUpload();
            //    minoFileUpload.Main();

            //ReadAndUpdateTemplate();
            //GenerateCodeFromJSON();
            CreateContentCategoriesJSON();
            MessageBox.Show("Done");
        }
        static double Bytes(double kilobytes)
        {
            double Bytes = 0;

            // calculates Bytes
            // 1 KB = 1024 bytes
            Bytes = kilobytes * 1024;

            return Bytes;
        }

        void ReadAndUpdateTemplate()
        {
            string filePath = @"C:\Users\Sandeep\Desktop\my_world_new\Templates_MOD\user_content_template-template.json";

            ContentTemplate contentTemplate = new ContentTemplate();

            var jsonContent = File.ReadAllText(filePath);
            if (!string.IsNullOrEmpty(jsonContent))
            {
                contentTemplate = JsonConvert.DeserializeObject<ContentTemplate>(jsonContent);
            }

            string XMLFolderPath = @"E:\AppDumps\my_book\TemplateXMLs";

            DirectoryInfo xmlDir = new DirectoryInfo(XMLFolderPath);

            var xmlList = Directory.GetFiles(xmlDir.FullName);


            foreach (var xmlItem in xmlList)
            {
                string contentType = System.IO.Path.GetFileNameWithoutExtension(xmlItem);

                var xmlContent = File.ReadAllText(xmlItem);
                xmlContent = xmlContent.Replace("<br>", "");

                var content = contentTemplate.Contents.Find(c => c.content_type == contentType);

                if (content != null)
                {
                    TextReader tr = new StringReader(xmlContent);
                    Ul xmlString = (Ul)XmlUtility.FromXml<Ul>(tr);

                    if (xmlString.Li != null && xmlString.Li.Count > 0)
                    {
                        foreach (var liItem in xmlString.Li)
                        {
                            string iconFormat = "<i class='{0}' translate='{1}'>{2}</i>";
                            var headerDiv = liItem.Div.Find(d => d.Class.Contains("collapsible-header"));
                            var bodyDiv = liItem.Div.Find(d => d.Class.Contains("collapsible-body"));

                            var category = content.categories.Find(c => c.Name == headerDiv.Text.ToLower().RemoveSpecialCharacters());
                            if (category != null)
                            {
                                category.Icon = string.Format(iconFormat, headerDiv.I.Class, headerDiv.I.Translate, headerDiv.I.Text);
                                if (bodyDiv.Divs != null && bodyDiv.Divs.Count > 0)
                                {
                                    for (int i = 0; i < bodyDiv.Divs.Count; i++)
                                    {
                                        var attribute = category.Attributes.Find(a => a.field_label.ToLower() == bodyDiv.Divs[i].Strong.ToLower());
                                        if (attribute != null)
                                            attribute.help_text = bodyDiv.Divs[++i].Text.RemoveSpecialCharactersExcludingSpace().Trim();
                                        else ++i;
                                    }
                                }

                            }
                        }
                    }

                }
            }


            var jsonTemplate = JsonConvert.SerializeObject(contentTemplate, Formatting.Indented);
            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            "my_book", "Templates_MOD", "Content_Template" + DateTime.Now.ToShortDateString() + ".json"), jsonTemplate, true);
        }

        void CreateContentCategoriesJSON()
        {
            ContentTemplate contentTemplate = new ContentTemplate();
            contentTemplate.TemplateName = "Content Template";
            contentTemplate.Contents = new List<Content>();

            #region CODE
            #region buildings
            Content content_buildings = new Content("buildings", true);

            Category buildings_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute buildings_overview_Name = new Attribute("Name", "Name", "varchar", "What is this buildings name");
            Attribute buildings_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this building.", "universes");
            Attribute buildings_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this building");
            Attribute buildings_overview_Type_of_building = new Attribute("Type_of_building", "Type Of Building", "tinytext", "What kind of building is this building");
            Attribute buildings_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "tinytext", "What other names is this building known by");
            Attribute buildings_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your buildings.");
            buildings_overview.Attributes.Add(buildings_overview_Name);
            buildings_overview.Attributes.Add(buildings_overview_Universe);
            buildings_overview.Attributes.Add(buildings_overview_Description);
            buildings_overview.Attributes.Add(buildings_overview_Type_of_building);
            buildings_overview.Attributes.Add(buildings_overview_Alternate_names);
            buildings_overview.Attributes.Add(buildings_overview_Tags);


            Category buildings_occupants = new Category(2, "Occupants", "<i class='material-icons' translate='no'>recent_actors</i>", "occupants");

            Attribute buildings_occupants_Owner = new Attribute("Owner", "Owner", "tinytext", "Who owns this building");
            Attribute buildings_occupants_Tenants = new Attribute("Tenants", "Tenants", "tinytext", "Who lives or works in this building");
            Attribute buildings_occupants_Affiliation = new Attribute("Affiliation", "Affiliation", "tinytext", "What groups or organizations is this building affiliated with");
            Attribute buildings_occupants_Capacity = new Attribute("Capacity", "Capacity", "int", "How many people can this building hold");
            buildings_occupants.Attributes.Add(buildings_occupants_Owner);
            buildings_occupants.Attributes.Add(buildings_occupants_Tenants);
            buildings_occupants.Attributes.Add(buildings_occupants_Affiliation);
            buildings_occupants.Attributes.Add(buildings_occupants_Capacity);


            Category buildings_design = new Category(3, "Design", "<i class='material-icons' translate='no'>format_paint</i>", "design");

            Attribute buildings_design_Facade = new Attribute("Facade", "Facade", "tinytext", "What does the outside of this building look like");
            Attribute buildings_design_Floor_count = new Attribute("Floor_count", "Floor Count", "int", "How many floors are there in this building");
            Attribute buildings_design_Dimensions = new Attribute("Dimensions", "Dimensions", "int", "How big is this building");
            Attribute buildings_design_Architectural_style = new Attribute("Architectural_style", "Architectural Style", "tinytext", "What style of architecture is this building");
            buildings_design.Attributes.Add(buildings_design_Facade);
            buildings_design.Attributes.Add(buildings_design_Floor_count);
            buildings_design.Attributes.Add(buildings_design_Dimensions);
            buildings_design.Attributes.Add(buildings_design_Architectural_style);


            Category buildings_usage = new Category(4, "Usage", "<i class='material-icons' translate='no'>extension</i>", "usage");

            Attribute buildings_usage_Permits = new Attribute("Permits", "Permits", "tinytext", "What permits does this building have");
            Attribute buildings_usage_Purpose = new Attribute("Purpose", "Purpose", "tinytext", "Why does this building exist What is it for");
            buildings_usage.Attributes.Add(buildings_usage_Permits);
            buildings_usage.Attributes.Add(buildings_usage_Purpose);


            Category buildings_location = new Category(5, "Location", "<i class='material-icons' translate='no'>location_on</i>", "location");

            Attribute buildings_location_Address = new Attribute("Address", "Address", "tinytext", "What is this buildings address");
            buildings_location.Attributes.Add(buildings_location_Address);


            Category buildings_neighborhood = new Category(6, "Neighborhood", "<i class='material-icons' translate='no'>store_mall_directory</i>", "neighborhood");



            Category buildings_financial = new Category(7, "Financial", "<i class='material-icons' translate='no'>attach_money</i>", "financial");

            Attribute buildings_financial_Price = new Attribute("Price", "Price", "double", "");
            buildings_financial.Attributes.Add(buildings_financial_Price);


            Category buildings_amenities = new Category(8, "Amenities", "<i class='material-icons' translate='no'>pool</i>", "amenities");



            Category buildings_history = new Category(9, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute buildings_history_Architect = new Attribute("Architect", "Architect", "tinytext", "Who designed this building");
            Attribute buildings_history_Developer = new Attribute("Developer", "Developer", "tinytext", "Who was the developer that built this building");
            Attribute buildings_history_Notable_events = new Attribute("Notable_events", "Notable Events", "longtext", "What has happened in or around this building");
            Attribute buildings_history_Constructed_year = new Attribute("Constructed_year", "Constructed Year", "int", "When was this building built");
            Attribute buildings_history_Construction_cost = new Attribute("Construction_cost", "Construction Cost", "double", "How much did it cost to build this building");
            buildings_history.Attributes.Add(buildings_history_Architect);
            buildings_history.Attributes.Add(buildings_history_Developer);
            buildings_history.Attributes.Add(buildings_history_Notable_events);
            buildings_history.Attributes.Add(buildings_history_Constructed_year);
            buildings_history.Attributes.Add(buildings_history_Construction_cost);


            Category buildings_notes = new Category(11, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute buildings_notes_Notes = new Attribute("Notes", "Notes", "longtext", "Write as little or as much as you want");
            Attribute buildings_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            buildings_notes.Attributes.Add(buildings_notes_Notes);
            buildings_notes.Attributes.Add(buildings_notes_Private_Notes);

            Category buildings_gallery = new Category(10, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute buildings_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            buildings_gallery.Attributes.Add(buildings_gallery_gallery);


            content_buildings.categories.Add(buildings_overview);
            content_buildings.categories.Add(buildings_occupants);
            content_buildings.categories.Add(buildings_design);
            content_buildings.categories.Add(buildings_usage);
            content_buildings.categories.Add(buildings_location);
            content_buildings.categories.Add(buildings_neighborhood);
            content_buildings.categories.Add(buildings_financial);
            content_buildings.categories.Add(buildings_amenities);
            content_buildings.categories.Add(buildings_history);
            content_buildings.categories.Add(buildings_notes);
            content_buildings.references.Add(buildings_gallery);


            content_buildings.categories = content_buildings.categories.OrderBy(c => c.Order).ToList();
            content_buildings.categories.AddAutoIncrmentId("Id");

            #endregion
            #region characters
            Content content_characters = new Content("characters", true);

            Category characters_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute characters_overview_Name = new Attribute("Name", "Name", "varchar", "What is this characters full name");
            Attribute characters_overview_Role = new Attribute("Role", "Role", "varchar", "What is this characters role in your story");
            Attribute characters_overview_Gender = new Attribute("Gender", "Gender", "varchar", "What is this characters gender");
            Attribute characters_overview_Age = new Attribute("Age", "Age", "varchar", "How old is this character");
            Attribute characters_overview_Universe = new Attribute("Universe", "Universe", "int", "This field allows you to link your other Notebook.ai pages to this character.", "universes");
            Attribute characters_overview_Aliases = new Attribute("Aliases", "Aliases", "text", "");
            Attribute characters_overview_Favorite = new Attribute("Favorite", "Favorite", "tinyint", "");
            Attribute characters_overview_Privacy = new Attribute("Privacy", "Privacy", "tinyint", "");
            characters_overview.Attributes.Add(characters_overview_Name);
            characters_overview.Attributes.Add(characters_overview_Role);
            characters_overview.Attributes.Add(characters_overview_Gender);
            characters_overview.Attributes.Add(characters_overview_Age);
            characters_overview.Attributes.Add(characters_overview_Universe);
            characters_overview.Attributes.Add(characters_overview_Aliases);
            characters_overview.Attributes.Add(characters_overview_Favorite);
            characters_overview.Attributes.Add(characters_overview_Privacy);


            Category characters_looks = new Category(2, "Looks", "<i class='material-icons' translate='no'>face</i>", "looks");

            Attribute characters_looks_Height = new Attribute("Height", "Height", "varchar", "How tall is this character");
            Attribute characters_looks_Weight = new Attribute("Weight", "Weight", "varchar", "How much does this character weigh");
            Attribute characters_looks_Haircolor = new Attribute("Haircolor", "Hair Color", "varchar", "What color is this characters hair");
            Attribute characters_looks_Hairstyle = new Attribute("Hairstyle", "Hair Style", "varchar", "How does this character style their hair");
            Attribute characters_looks_Facialhair = new Attribute("Facialhair", "Facial Hair", "varchar", "What facial hair does this character have");
            Attribute characters_looks_Eyecolor = new Attribute("Eyecolor", "Eye Color", "varchar", "What is this characters eye color");
            Attribute characters_looks_Race = new Attribute("Race", "Race", "varchar", "What is this characters race");
            Attribute characters_looks_Skintone = new Attribute("Skintone", "Skin Tone", "varchar", "Write as little or as much as you want");
            Attribute characters_looks_Bodytype = new Attribute("Bodytype", "Body Type", "varchar", "Write as little or as much as you want");
            Attribute characters_looks_Identmarks = new Attribute("Identmarks", "Identifying Marks", "text", "What identifying marks does this character have");
            characters_looks.Attributes.Add(characters_looks_Height);
            characters_looks.Attributes.Add(characters_looks_Weight);
            characters_looks.Attributes.Add(characters_looks_Haircolor);
            characters_looks.Attributes.Add(characters_looks_Hairstyle);
            characters_looks.Attributes.Add(characters_looks_Facialhair);
            characters_looks.Attributes.Add(characters_looks_Eyecolor);
            characters_looks.Attributes.Add(characters_looks_Race);
            characters_looks.Attributes.Add(characters_looks_Skintone);
            characters_looks.Attributes.Add(characters_looks_Bodytype);
            characters_looks.Attributes.Add(characters_looks_Identmarks);


            Category characters_nature = new Category(3, "Nature", "<i class='material-icons' translate='no'>fingerprint</i>", "nature");

            Attribute characters_nature_Mannerisms = new Attribute("Mannerisms", "Mannerisms", "text", "What mannerisms does this character have");
            Attribute characters_nature_Motivations = new Attribute("Motivations", "Motivations", "text", "What motivates this character most");
            Attribute characters_nature_Flaws = new Attribute("Flaws", "Flaws", "text", "What flaws does this character have");
            Attribute characters_nature_Talents = new Attribute("Talents", "Talents", "text", "What talents does this character have");
            Attribute characters_nature_Hobbies = new Attribute("Hobbies", "Hobbies", "text", "What hobbies does this character have");
            Attribute characters_nature_Personality_type = new Attribute("Personality_type", "Personality Type", "text", "What personality type is this character");
            characters_nature.Attributes.Add(characters_nature_Mannerisms);
            characters_nature.Attributes.Add(characters_nature_Motivations);
            characters_nature.Attributes.Add(characters_nature_Flaws);
            characters_nature.Attributes.Add(characters_nature_Talents);
            characters_nature.Attributes.Add(characters_nature_Hobbies);
            characters_nature.Attributes.Add(characters_nature_Personality_type);


            Category characters_social = new Category(4, "Social", "<i class='material-icons' translate='no'>groups</i>", "social");

            Attribute characters_social_Religion = new Attribute("Religion", "Religion", "text", "What religion does this character practice");
            Attribute characters_social_Politics = new Attribute("Politics", "Politics", "text", "What politics does this character have");
            Attribute characters_social_Prejudices = new Attribute("Prejudices", "Prejudices", "text", "");
            Attribute characters_social_Occupation = new Attribute("Occupation", "Occupation", "text", "What is this characters occupation");
            Attribute characters_social_Fave_color = new Attribute("Fave_color", "Fave Color", "varchar", "");
            Attribute characters_social_Fave_food = new Attribute("Fave_food", "Fave Food", "varchar", "");
            Attribute characters_social_Fave_possession = new Attribute("Fave_possession", "Fave Possession", "varchar", "");
            Attribute characters_social_Fave_weapon = new Attribute("Fave_weapon", "Fave Weapon", "varchar", "");
            Attribute characters_social_Fave_animal = new Attribute("Fave_animal", "Fave Animal", "varchar", "");
            characters_social.Attributes.Add(characters_social_Religion);
            characters_social.Attributes.Add(characters_social_Politics);
            characters_social.Attributes.Add(characters_social_Prejudices);
            characters_social.Attributes.Add(characters_social_Occupation);
            characters_social.Attributes.Add(characters_social_Fave_color);
            characters_social.Attributes.Add(characters_social_Fave_food);
            characters_social.Attributes.Add(characters_social_Fave_possession);
            characters_social.Attributes.Add(characters_social_Fave_weapon);
            characters_social.Attributes.Add(characters_social_Fave_animal);


            Category characters_history = new Category(5, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute characters_history_Birthday = new Attribute("Birthday", "Birthday", "text", "When is this characters birthday");
            Attribute characters_history_Birthplace = new Attribute("Birthplace", "Birthplace", "text", "This field allows you to link your other Notebook.ai pages to this character.");
            Attribute characters_history_Education = new Attribute("Education", "Education", "text", "What is this characters level of education");
            Attribute characters_history_Background = new Attribute("Background", "Background", "text", "What is this characters background");
            characters_history.Attributes.Add(characters_history_Birthday);
            characters_history.Attributes.Add(characters_history_Birthplace);
            characters_history.Attributes.Add(characters_history_Education);
            characters_history.Attributes.Add(characters_history_Background);


            Category characters_family = new Category(6, "Family", "<i class='material-icons' translate='no'>device_hub</i>", "family");

            Attribute characters_family_Pets = new Attribute("Pets", "Pets", "text", "What pets does this character have");
            characters_family.Attributes.Add(characters_family_Pets);


            Category characters_inventory = new Category(7, "Inventory", "<i class='material-icons' translate='no'>shopping_basket</i>", "inventory");



            Category characters_notes = new Category(9, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute characters_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute characters_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", "Write as little or as much as you want");
            characters_notes.Attributes.Add(characters_notes_Notes);
            characters_notes.Attributes.Add(characters_notes_Private_notes);

            Category characters_gallery = new Category(8, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute characters_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            characters_gallery.Attributes.Add(characters_gallery_gallery);


            content_characters.categories.Add(characters_overview);
            content_characters.categories.Add(characters_looks);
            content_characters.categories.Add(characters_nature);
            content_characters.categories.Add(characters_social);
            content_characters.categories.Add(characters_history);
            content_characters.categories.Add(characters_family);
            content_characters.categories.Add(characters_inventory);
            content_characters.categories.Add(characters_notes);
            content_characters.references.Add(characters_gallery);


            content_characters.categories = content_characters.categories.OrderBy(c => c.Order).ToList();
            content_characters.categories.AddAutoIncrmentId("Id");

            #endregion
            #region conditions
            Content content_conditions = new Content("conditions", true);

            Category conditions_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute conditions_overview_Name = new Attribute("Name", "Name", "varchar", "What is this conditions name");
            Attribute conditions_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this condition.", "universes");
            Attribute conditions_overview_Description = new Attribute("Description", "Description", "text", "Describe this condition.");
            Attribute conditions_overview_Type_of_condition = new Attribute("Type_of_condition", "Type Of Condition", "varchar", "What kind of condition is this condition");
            Attribute conditions_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "varchar", "What other names is this condition known by");
            Attribute conditions_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your conditions.");
            conditions_overview.Attributes.Add(conditions_overview_Name);
            conditions_overview.Attributes.Add(conditions_overview_Universe);
            conditions_overview.Attributes.Add(conditions_overview_Description);
            conditions_overview.Attributes.Add(conditions_overview_Type_of_condition);
            conditions_overview.Attributes.Add(conditions_overview_Alternate_names);
            conditions_overview.Attributes.Add(conditions_overview_Tags);


            Category conditions_causes = new Category(2, "Causes", "<i class='material-icons' translate='no'>bubble_chart</i>", "causes");

            Attribute conditions_causes_Genetic_factors = new Attribute("Genetic_factors", "Genetic Factors", "varchar", "What genetic factors affect how contractable or effective this condition is");
            Attribute conditions_causes_Environmental_factors = new Attribute("Environmental_factors", "Environmental Factors", "varchar", "What environmental factors affect how contractable or effective this condition is");
            Attribute conditions_causes_Lifestyle_factors = new Attribute("Lifestyle_factors", "Lifestyle Factors", "varchar", "What lifestyle factors affect how contractable or effective this condition is");
            Attribute conditions_causes_Transmission = new Attribute("Transmission", "Transmission", "varchar", "How does this condition spread");
            Attribute conditions_causes_Epidemiology = new Attribute("Epidemiology", "Epidemiology", "varchar", "How is this condition controlled or quarantined How does it first occur");
            conditions_causes.Attributes.Add(conditions_causes_Genetic_factors);
            conditions_causes.Attributes.Add(conditions_causes_Environmental_factors);
            conditions_causes.Attributes.Add(conditions_causes_Lifestyle_factors);
            conditions_causes.Attributes.Add(conditions_causes_Transmission);
            conditions_causes.Attributes.Add(conditions_causes_Epidemiology);


            Category conditions_effects = new Category(3, "Effects", "<i class='material-icons' translate='no'>local_hospital</i>", "effects");

            Attribute conditions_effects_Visual_effects = new Attribute("Visual_effects", "Visual Effects", "varchar", "How does this condition manifest visually");
            Attribute conditions_effects_Mental_effects = new Attribute("Mental_effects", "Mental Effects", "varchar", "How does this condition affect the mind");
            Attribute conditions_effects_Symptoms = new Attribute("Symptoms", "Symptoms", "varchar", "What are the symptoms of this condition");
            Attribute conditions_effects_Duration = new Attribute("Duration", "Duration", "varchar", "How long does this condition last");
            Attribute conditions_effects_Prognosis = new Attribute("Prognosis", "Prognosis", "varchar", "How deadly is this condition What is the most likely outcome for those who contract it");
            Attribute conditions_effects_Variations = new Attribute("Variations", "Variations", "varchar", "What other forms of this condition are out there");
            conditions_effects.Attributes.Add(conditions_effects_Visual_effects);
            conditions_effects.Attributes.Add(conditions_effects_Mental_effects);
            conditions_effects.Attributes.Add(conditions_effects_Symptoms);
            conditions_effects.Attributes.Add(conditions_effects_Duration);
            conditions_effects.Attributes.Add(conditions_effects_Prognosis);
            conditions_effects.Attributes.Add(conditions_effects_Variations);


            Category conditions_treatment = new Category(4, "Treatment", "<i class='material-icons' translate='no'>healing</i>", "treatment");

            Attribute conditions_treatment_Prevention = new Attribute("Prevention", "Prevention", "varchar", "How do people keep from contracting this condition");
            Attribute conditions_treatment_Diagnostic_method = new Attribute("Diagnostic_method", "Diagnostic Method", "varchar", "How do doctors determine whether someone has this condition");
            Attribute conditions_treatment_Treatment = new Attribute("Treatment", "Treatment", "varchar", "What is the treatment for this condition");
            Attribute conditions_treatment_Medication = new Attribute("Medication", "Medication", "varchar", "What medicines help treat this condition");
            Attribute conditions_treatment_Immunization = new Attribute("Immunization", "Immunization", "varchar", "What immunizations are available for this condition");
            conditions_treatment.Attributes.Add(conditions_treatment_Prevention);
            conditions_treatment.Attributes.Add(conditions_treatment_Diagnostic_method);
            conditions_treatment.Attributes.Add(conditions_treatment_Treatment);
            conditions_treatment.Attributes.Add(conditions_treatment_Medication);
            conditions_treatment.Attributes.Add(conditions_treatment_Immunization);


            Category conditions_analysis = new Category(5, "Analysis", "<i class='material-icons' translate='no'>bar_chart</i>", "analysis");

            Attribute conditions_analysis_Specialty_Field = new Attribute("Specialty_Field", "Specialty", "varchar", "What specialtyfield is most relevant to understanding this condition");
            Attribute conditions_analysis_Rarity = new Attribute("Rarity", "Rarity", "varchar", "How rare is this condition");
            Attribute conditions_analysis_Symbolism = new Attribute("Symbolism", "Symbolism", "varchar", "What does this condition represent in your world");
            conditions_analysis.Attributes.Add(conditions_analysis_Specialty_Field);
            conditions_analysis.Attributes.Add(conditions_analysis_Rarity);
            conditions_analysis.Attributes.Add(conditions_analysis_Symbolism);


            Category conditions_history = new Category(6, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute conditions_history_Origin = new Attribute("Origin", "Origin", "text", "Where did this condition originate");
            Attribute conditions_history_Evolution = new Attribute("Evolution", "Evolution", "varchar", "How has this condition changed over time");
            conditions_history.Attributes.Add(conditions_history_Origin);
            conditions_history.Attributes.Add(conditions_history_Evolution);


            Category conditions_notes = new Category(8, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute conditions_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute conditions_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            conditions_notes.Attributes.Add(conditions_notes_Notes);
            conditions_notes.Attributes.Add(conditions_notes_Private_Notes);

            Category conditions_gallery = new Category(7, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute conditions_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            conditions_gallery.Attributes.Add(conditions_gallery_gallery);


            content_conditions.categories.Add(conditions_overview);
            content_conditions.categories.Add(conditions_causes);
            content_conditions.categories.Add(conditions_effects);
            content_conditions.categories.Add(conditions_treatment);
            content_conditions.categories.Add(conditions_analysis);
            content_conditions.categories.Add(conditions_history);
            content_conditions.categories.Add(conditions_notes);
            content_conditions.references.Add(conditions_gallery);


            content_conditions.categories = content_conditions.categories.OrderBy(c => c.Order).ToList();
            content_conditions.categories.AddAutoIncrmentId("Id");

            #endregion
            #region continents
            Content content_continents = new Content("continents", true);

            Category continents_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute continents_overview_Name = new Attribute("Name", "Name", "varchar", "What is this continent name");
            Attribute continents_overview_Local_name = new Attribute("Local_name", "Local Name", "text", "What do the people of this continent call it");
            Attribute continents_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this continent called");
            Attribute continents_overview_Description = new Attribute("Description", "Description", "text", "What is this continents description");
            Attribute continents_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this continent.", "universes");
            Attribute continents_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your continents.");
            continents_overview.Attributes.Add(continents_overview_Name);
            continents_overview.Attributes.Add(continents_overview_Local_name);
            continents_overview.Attributes.Add(continents_overview_Other_Names);
            continents_overview.Attributes.Add(continents_overview_Description);
            continents_overview.Attributes.Add(continents_overview_Universe);
            continents_overview.Attributes.Add(continents_overview_Tags);


            Category continents_geography = new Category(2, "Geography", "<i class='material-icons' translate='no'>terrain</i>", "geography");

            Attribute continents_geography_Area = new Attribute("Area", "Area", "double", "How large is this continent");
            Attribute continents_geography_Shape = new Attribute("Shape", "Shape", "tinytext", "What is this continent shaped like");
            Attribute continents_geography_Population = new Attribute("Population", "Population", "tinytext", "How many people live in this continent");
            Attribute continents_geography_Topography = new Attribute("Topography", "Topography", "tinytext", "How would you describe the topography of this continent");
            Attribute continents_geography_Mineralogy = new Attribute("Mineralogy", "Mineralogy", "tinytext", "What minerals or other kinds of rocks are there in this continent");
            Attribute continents_geography_Bodies_of_water = new Attribute("Bodies_of_water", "Bodies Of Water", "tinytext", "What large bodies of water are there in this continent");
            Attribute continents_geography_Landmarks = new Attribute("Landmarks", "Landmarks", "tinytext", "This field allows you to link your other Notebook.ai pages to this continent.");
            Attribute continents_geography_Regional_advantages = new Attribute("Regional_advantages", "Regional Advantages", "tinytext", "What regional advantages does this continent have");
            Attribute continents_geography_Regional_disadvantages = new Attribute("Regional_disadvantages", "Regional Disadvantages", "tinytext", "What regional disadvantages does this continent have");
            continents_geography.Attributes.Add(continents_geography_Area);
            continents_geography.Attributes.Add(continents_geography_Shape);
            continents_geography.Attributes.Add(continents_geography_Population);
            continents_geography.Attributes.Add(continents_geography_Topography);
            continents_geography.Attributes.Add(continents_geography_Mineralogy);
            continents_geography.Attributes.Add(continents_geography_Bodies_of_water);
            continents_geography.Attributes.Add(continents_geography_Landmarks);
            continents_geography.Attributes.Add(continents_geography_Regional_advantages);
            continents_geography.Attributes.Add(continents_geography_Regional_disadvantages);


            Category continents_culture = new Category(3, "Culture", "<i class='material-icons' translate='no'>face</i>", "culture");

            Attribute continents_culture_Demonym = new Attribute("Demonym", "Demonym", "tinytext", "What are the people that live in this continent called");
            Attribute continents_culture_Politics = new Attribute("Politics", "Politics", "tinytext", "What are the politics like in this continent");
            Attribute continents_culture_Economy = new Attribute("Economy", "Economy", "tinytext", "What is the economy like in this continent");
            Attribute continents_culture_Tourism = new Attribute("Tourism", "Tourism", "tinytext", "What is the tourism like in this continent");
            Attribute continents_culture_Architecture = new Attribute("Architecture", "Architecture", "tinytext", "What is the architectural style in this continent");
            Attribute continents_culture_Reputation = new Attribute("Reputation", "Reputation", "tinytext", "What is this continents reputation");
            Attribute continents_culture_Countries = new Attribute("Countries", "Countries", "tinytext", "This field allows you to link your other Notebook.ai pages to this continent.");
            Attribute continents_culture_Languages = new Attribute("Languages", "Languages", "tinytext", "This field allows you to link your other Notebook.ai pages to this continent.");
            Attribute continents_culture_Traditions = new Attribute("Traditions", "Traditions", "tinytext", "This field allows you to link your other Notebook.ai pages to this continent.");
            Attribute continents_culture_Governments = new Attribute("Governments", "Governments", "tinytext", "This field allows you to link your other Notebook.ai pages to this continent.");
            Attribute continents_culture_Popular_foods = new Attribute("Popular_foods", "Popular Foods", "tinytext", "This field allows you to link your other Notebook.ai pages to this continent.");
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


            Category continents_nature = new Category(4, "Nature", "<i class='material-icons' translate='no'>local_florist</i>", "nature");

            Attribute continents_nature_Crops = new Attribute("Crops", "Crops", "tinytext", "What crops are usually grown in this continent");
            Attribute continents_nature_Creatures = new Attribute("Creatures", "Creatures", "tinytext", "This field allows you to link your other Notebook.ai pages to this continent.");
            Attribute continents_nature_Floras = new Attribute("Floras", "Floras", "tinytext", "This field allows you to link your other Notebook.ai pages to this continent.");
            continents_nature.Attributes.Add(continents_nature_Crops);
            continents_nature.Attributes.Add(continents_nature_Creatures);
            continents_nature.Attributes.Add(continents_nature_Floras);


            Category continents_climate = new Category(5, "Climate", "<i class='material-icons' translate='no'>fireplace</i>", "climate");

            Attribute continents_climate_Temperature = new Attribute("Temperature", "Temperature", "tinytext", "How hot does it get in this continent How cold");
            Attribute continents_climate_Seasons = new Attribute("Seasons", "Seasons", "tinytext", "What are the seasons like in this continent");
            Attribute continents_climate_Humidity = new Attribute("Humidity", "Humidity", "tinytext", "How humid is it in this continent");
            Attribute continents_climate_Precipitation = new Attribute("Precipitation", "Precipitation", "tinytext", "What kind of precipitation does this continent experience How often");
            Attribute continents_climate_Winds = new Attribute("Winds", "Winds", "tinytext", "What are the winds like in this continent");
            Attribute continents_climate_Natural_disasters = new Attribute("Natural_disasters", "Natural Disasters", "tinytext", "What natural disasters are common in this continent");
            continents_climate.Attributes.Add(continents_climate_Temperature);
            continents_climate.Attributes.Add(continents_climate_Seasons);
            continents_climate.Attributes.Add(continents_climate_Humidity);
            continents_climate.Attributes.Add(continents_climate_Precipitation);
            continents_climate.Attributes.Add(continents_climate_Winds);
            continents_climate.Attributes.Add(continents_climate_Natural_disasters);


            Category continents_history = new Category(6, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute continents_history_Formation = new Attribute("Formation", "Formation", "tinytext", "How did this continent originally form");
            Attribute continents_history_Discovery = new Attribute("Discovery", "Discovery", "tinytext", "How was this continent discovered");
            Attribute continents_history_Wars = new Attribute("Wars", "Wars", "tinytext", "What wars have been fought in this continent");
            Attribute continents_history_Ruins = new Attribute("Ruins", "Ruins", "tinytext", "What ruins are there in this continent");
            continents_history.Attributes.Add(continents_history_Formation);
            continents_history.Attributes.Add(continents_history_Discovery);
            continents_history.Attributes.Add(continents_history_Wars);
            continents_history.Attributes.Add(continents_history_Ruins);


            Category continents_notes = new Category(8, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute continents_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute continents_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            continents_notes.Attributes.Add(continents_notes_Notes);
            continents_notes.Attributes.Add(continents_notes_Private_Notes);

            Category continents_gallery = new Category(7, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute continents_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            continents_gallery.Attributes.Add(continents_gallery_gallery);


            content_continents.categories.Add(continents_overview);
            content_continents.categories.Add(continents_geography);
            content_continents.categories.Add(continents_culture);
            content_continents.categories.Add(continents_nature);
            content_continents.categories.Add(continents_climate);
            content_continents.categories.Add(continents_history);
            content_continents.categories.Add(continents_notes);
            content_continents.references.Add(continents_gallery);


            content_continents.categories = content_continents.categories.OrderBy(c => c.Order).ToList();
            content_continents.categories.AddAutoIncrmentId("Id");

            #endregion
            #region countries
            Content content_countries = new Content("countries", true);

            Category countries_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute countries_overview_Name = new Attribute("Name", "Name", "varchar", "Whats the name of this country");
            Attribute countries_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this country");
            Attribute countries_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this country known by");
            Attribute countries_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this country.", "universes");
            Attribute countries_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your countries.");
            countries_overview.Attributes.Add(countries_overview_Name);
            countries_overview.Attributes.Add(countries_overview_Description);
            countries_overview.Attributes.Add(countries_overview_Other_Names);
            countries_overview.Attributes.Add(countries_overview_Universe);
            countries_overview.Attributes.Add(countries_overview_Tags);


            Category countries_points_of_interest = new Category(2, "Points Of Interest", "<i class='material-icons' translate='no'>info</i>", "points_of_interest");

            Attribute countries_points_of_interest_Locations = new Attribute("Locations", "Locations", "varchar", "");
            Attribute countries_points_of_interest_Towns = new Attribute("Towns", "Towns", "varchar", "");
            Attribute countries_points_of_interest_Landmarks = new Attribute("Landmarks", "Landmarks", "varchar", "");
            Attribute countries_points_of_interest_Bordering_countries = new Attribute("Bordering_countries", "Bordering Countries", "varchar", "");
            countries_points_of_interest.Attributes.Add(countries_points_of_interest_Locations);
            countries_points_of_interest.Attributes.Add(countries_points_of_interest_Towns);
            countries_points_of_interest.Attributes.Add(countries_points_of_interest_Landmarks);
            countries_points_of_interest.Attributes.Add(countries_points_of_interest_Bordering_countries);


            Category countries_culture = new Category(3, "Culture", "<i class='material-icons' translate='no'>face</i>", "culture");

            Attribute countries_culture_Population = new Attribute("Population", "Population", "double", "What is the population of this country");
            Attribute countries_culture_Social_hierarchy = new Attribute("Social_hierarchy", "Social Hierarchy", "tinytext", "How is the population separated into hierarchical classes");
            Attribute countries_culture_Currency = new Attribute("Currency", "Currency", "varchar", "What currency is used in this country");
            Attribute countries_culture_Laws = new Attribute("Laws", "Laws", "tinytext", "What are the major laws in this country");
            Attribute countries_culture_Pop_culture = new Attribute("Pop_culture", "Pop Culture", "tinytext", "What is the pop culture scene like in this country");
            Attribute countries_culture_Music = new Attribute("Music", "Music", "tinytext", "What kinds of music are popular in this country");
            Attribute countries_culture_Education = new Attribute("Education", "Education", "varchar", "How important is education in this country");
            Attribute countries_culture_Architecture = new Attribute("Architecture", "Architecture", "tinytext", "What kind of architecture is popular in this country");
            Attribute countries_culture_Sports = new Attribute("Sports", "Sports", "varchar", "What sports are popular in this country");
            Attribute countries_culture_Languages = new Attribute("Languages", "Languages", "varchar", "This field allows you to link your other Notebook.ai pages to this country.");
            Attribute countries_culture_Religions = new Attribute("Religions", "Religions", "varchar", "This field allows you to link your other Notebook.ai pages to this country.");
            Attribute countries_culture_Governments = new Attribute("Governments", "Governments", "varchar", "This field allows you to link your other Notebook.ai pages to this country.");
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


            Category countries_geography = new Category(4, "Geography", "<i class='material-icons' translate='no'>terrain</i>", "geography");

            Attribute countries_geography_Area = new Attribute("Area", "Area", "double", "How big is this country");
            Attribute countries_geography_Crops = new Attribute("Crops", "Crops", "tinytext", "What crops does this country import or export");
            Attribute countries_geography_Climate = new Attribute("Climate", "Climate", "tinytext", "What is the climate like in this country");
            Attribute countries_geography_Creatures = new Attribute("Creatures", "Creatures", "tinytext", "This field allows you to link your other Notebook.ai pages to this country.");
            Attribute countries_geography_Flora = new Attribute("Flora", "Flora", "tinytext", "This field allows you to link your other Notebook.ai pages to this country.");
            countries_geography.Attributes.Add(countries_geography_Area);
            countries_geography.Attributes.Add(countries_geography_Crops);
            countries_geography.Attributes.Add(countries_geography_Climate);
            countries_geography.Attributes.Add(countries_geography_Creatures);
            countries_geography.Attributes.Add(countries_geography_Flora);


            Category countries_history = new Category(5, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute countries_history_Founding_story = new Attribute("Founding_story", "Founding Story", "tinytext", "How was this country founded");
            Attribute countries_history_Established_year = new Attribute("Established_year", "Established Year", "int", "When was this country founded");
            Attribute countries_history_Notable_wars = new Attribute("Notable_wars", "Notable Wars", "tinytext", "What notable wars has this country participated in");
            countries_history.Attributes.Add(countries_history_Founding_story);
            countries_history.Attributes.Add(countries_history_Established_year);
            countries_history.Attributes.Add(countries_history_Notable_wars);


            Category countries_notes = new Category(7, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute countries_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute countries_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            countries_notes.Attributes.Add(countries_notes_Notes);
            countries_notes.Attributes.Add(countries_notes_Private_Notes);

            Category countries_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute countries_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            countries_gallery.Attributes.Add(countries_gallery_gallery);


            content_countries.categories.Add(countries_overview);
            content_countries.categories.Add(countries_points_of_interest);
            content_countries.categories.Add(countries_culture);
            content_countries.categories.Add(countries_geography);
            content_countries.categories.Add(countries_history);
            content_countries.categories.Add(countries_notes);
            content_countries.references.Add(countries_gallery);


            content_countries.categories = content_countries.categories.OrderBy(c => c.Order).ToList();
            content_countries.categories.AddAutoIncrmentId("Id");

            #endregion
            #region creatures
            Content content_creatures = new Content("creatures", true);

            Category creatures_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute creatures_overview_Name = new Attribute("Name", "Name", "varchar", "Write as little or as much as you want");
            Attribute creatures_overview_Description = new Attribute("Description", "Description", "text", "How would you describe a this creature");
            Attribute creatures_overview_Type_of_creature = new Attribute("Type_of_creature", "Type Of Creature", "text", "What type of animal is this creature");
            Attribute creatures_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this creature.", "universes");
            Attribute creatures_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your creatures.");
            creatures_overview.Attributes.Add(creatures_overview_Name);
            creatures_overview.Attributes.Add(creatures_overview_Description);
            creatures_overview.Attributes.Add(creatures_overview_Type_of_creature);
            creatures_overview.Attributes.Add(creatures_overview_Universe);
            creatures_overview.Attributes.Add(creatures_overview_Tags);


            Category creatures_looks = new Category(2, "Looks", "<i class='material-icons' translate='no'>pets</i>", "looks");

            Attribute creatures_looks_Color = new Attribute("Color", "Color", "int", "What colors does a this creature come in");
            Attribute creatures_looks_Shape = new Attribute("Shape", "Shape", "tinytext", "How would you describe the shape of a this creature");
            Attribute creatures_looks_Size = new Attribute("Size", "Size", "double", "How big or small is the usual this creature");
            Attribute creatures_looks_Height = new Attribute("Height", "Height", "double", "How tall is the usual this creature");
            Attribute creatures_looks_Weight = new Attribute("Weight", "Weight", "double", "How much does the usual this creature weigh");
            Attribute creatures_looks_Notable_features = new Attribute("Notable_features", "Notable Features", "tinytext", "What physical features are most notable for this creature");
            Attribute creatures_looks_Vestigial_features = new Attribute("Vestigial_features", "Vestigial Features", "tinytext", "What vestigial features does this creature have");
            Attribute creatures_looks_Materials = new Attribute("Materials", "Materials", "tinytext", "What materials feathers scales etc is this creature made of");
            creatures_looks.Attributes.Add(creatures_looks_Color);
            creatures_looks.Attributes.Add(creatures_looks_Shape);
            creatures_looks.Attributes.Add(creatures_looks_Size);
            creatures_looks.Attributes.Add(creatures_looks_Height);
            creatures_looks.Attributes.Add(creatures_looks_Weight);
            creatures_looks.Attributes.Add(creatures_looks_Notable_features);
            creatures_looks.Attributes.Add(creatures_looks_Vestigial_features);
            creatures_looks.Attributes.Add(creatures_looks_Materials);


            Category creatures_traits = new Category(3, "Traits", "<i class='material-icons' translate='no'>fingerprint</i>", "traits");

            Attribute creatures_traits_Aggressiveness = new Attribute("Aggressiveness", "Aggressiveness", "tinytext", "How aggressive is the average this creature");
            Attribute creatures_traits_Method_of_attack = new Attribute("Method_of_attack", "Method Of Attack", "tinytext", "What methods does a this creature use to attack");
            Attribute creatures_traits_Methods_of_defense = new Attribute("Methods_of_defense", "Methods Of Defense", "tinytext", "How does a this creature defend itself from attackers");
            Attribute creatures_traits_Maximum_speed = new Attribute("Maximum_speed", "Maximum Speed", "double", "How fast can a this creature move");
            Attribute creatures_traits_Strengths = new Attribute("Strengths", "Strengths", "tinytext", "What are the notable strengths of this creature");
            Attribute creatures_traits_Weaknesses = new Attribute("Weaknesses", "Weaknesses", "tinytext", "What are the notable weaknesses of this creature");
            Attribute creatures_traits_Sounds = new Attribute("Sounds", "Sounds", "tinytext", "What sounds does this creature make");
            Attribute creatures_traits_Spoils = new Attribute("Spoils", "Spoils", "tinytext", "When hunted what spoils does a this creature leave behind");
            Attribute creatures_traits_Conditions = new Attribute("Conditions", "Conditions", "tinytext", "");
            Attribute creatures_traits_Weakest_sense = new Attribute("Weakest_sense", "Weakest Sense", "tinytext", "What is this creatures weakest sense");
            Attribute creatures_traits_Strongest_sense = new Attribute("Strongest_sense", "Strongest Sense", "tinytext", "What is this creatures strongest sense");
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


            Category creatures_habitat = new Category(4, "Habitat", "<i class='material-icons' translate='no'>location_on</i>", "habitat");

            Attribute creatures_habitat_Preferred_habitat = new Attribute("Preferred_habitat", "Preferred Habitat", "tinytext", "What kind of habitat is best for this creature");
            Attribute creatures_habitat_Habitats = new Attribute("Habitats", "Habitats", "tinytext", "This field allows you to link your other Notebook.ai pages to this creature.");
            Attribute creatures_habitat_Food_sources = new Attribute("Food_sources", "Food Sources", "tinytext", "Where does this creature find its food");
            Attribute creatures_habitat_Migratory_patterns = new Attribute("Migratory_patterns", "Migratory Patterns", "tinytext", "Does this creature have any migratory patterns");
            Attribute creatures_habitat_Herding_patterns = new Attribute("Herding_patterns", "Herding Patterns", "tinytext", "What herd patterns does this creature have");
            Attribute creatures_habitat_Competitors = new Attribute("Competitors", "Competitors", "tinytext", "What does this creature compete with for food or other resources in its habitat");
            Attribute creatures_habitat_Predators = new Attribute("Predators", "Predators", "tinytext", "What are the major predators of this creature");
            Attribute creatures_habitat_Prey = new Attribute("Prey", "Prey", "tinytext", "What does this creature prey on in its habitat");
            creatures_habitat.Attributes.Add(creatures_habitat_Preferred_habitat);
            creatures_habitat.Attributes.Add(creatures_habitat_Habitats);
            creatures_habitat.Attributes.Add(creatures_habitat_Food_sources);
            creatures_habitat.Attributes.Add(creatures_habitat_Migratory_patterns);
            creatures_habitat.Attributes.Add(creatures_habitat_Herding_patterns);
            creatures_habitat.Attributes.Add(creatures_habitat_Competitors);
            creatures_habitat.Attributes.Add(creatures_habitat_Predators);
            creatures_habitat.Attributes.Add(creatures_habitat_Prey);


            Category creatures_comparisons = new Category(5, "Comparisons", "<i class='material-icons' translate='no'>call_split</i>", "comparisons");

            Attribute creatures_comparisons_Similar_creatures = new Attribute("Similar_creatures", "Similar Creatures", "tinytext", "What other creatures is this creature most like");
            Attribute creatures_comparisons_Symbolisms = new Attribute("Symbolisms", "Symbolisms", "tinytext", "What symbolisms does this creature hold in your world");
            Attribute creatures_comparisons_Related_creatures = new Attribute("Related_creatures", "Related Creatures", "tinytext", "This field allows you to link your other Notebook.ai pages to this creature.");
            creatures_comparisons.Attributes.Add(creatures_comparisons_Similar_creatures);
            creatures_comparisons.Attributes.Add(creatures_comparisons_Symbolisms);
            creatures_comparisons.Attributes.Add(creatures_comparisons_Related_creatures);


            Category creatures_evolution = new Category(6, "Evolution", "<i class='material-icons' translate='no'>timeline</i>", "evolution");

            Attribute creatures_evolution_Ancestors = new Attribute("Ancestors", "Ancestors", "tinytext", "What preceded this creature How has this creature evolved over time");
            Attribute creatures_evolution_Evolutionary_drive = new Attribute("Evolutionary_drive", "Evolutionary Drive", "tinytext", "What drove this creature to evolve over time");
            Attribute creatures_evolution_Tradeoffs = new Attribute("Tradeoffs", "Tradeoffs", "tinytext", "What evolutionary tradeoffs has this creature made throughout history");
            Attribute creatures_evolution_Predictions = new Attribute("Predictions", "Predictions", "tinytext", "How might this creature further evolve in the future");
            creatures_evolution.Attributes.Add(creatures_evolution_Ancestors);
            creatures_evolution.Attributes.Add(creatures_evolution_Evolutionary_drive);
            creatures_evolution.Attributes.Add(creatures_evolution_Tradeoffs);
            creatures_evolution.Attributes.Add(creatures_evolution_Predictions);


            Category creatures_reproduction = new Category(7, "Reproduction", "<i class='material-icons' translate='no'>scatter_plot</i>", "reproduction");

            Attribute creatures_reproduction_Reproduction_age = new Attribute("Reproduction_age", "Reproduction Age", "double", "At what age can this creature start to reproduce");
            Attribute creatures_reproduction_Requirements = new Attribute("Requirements", "Requirements", "tinytext", "Besides age what other requirements are necessary before this creature can reproduce");
            Attribute creatures_reproduction_Mating_ritual = new Attribute("Mating_ritual", "Mating Ritual", "tinytext", "What is this creatures mating ritual");
            Attribute creatures_reproduction_Reproduction = new Attribute("Reproduction", "Reproduction", "tinytext", "How does this creature reproduce");
            Attribute creatures_reproduction_Reproduction_frequency = new Attribute("Reproduction_frequency", "Reproduction Frequency", "tinytext", "How frequently does this creature reproduce");
            Attribute creatures_reproduction_Parental_instincts = new Attribute("Parental_instincts", "Parental Instincts", "tinytext", "What parental instincts does this creature have");
            Attribute creatures_reproduction_Offspring_care = new Attribute("Offspring_care", "Offspring Care", "tinytext", "How does this creature take care of its offspring");
            Attribute creatures_reproduction_Mortality_rate = new Attribute("Mortality_rate", "Mortality Rate", "tinytext", "What percent of this creature offspring make it to adulthood");
            creatures_reproduction.Attributes.Add(creatures_reproduction_Reproduction_age);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Requirements);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Mating_ritual);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Reproduction);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Reproduction_frequency);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Parental_instincts);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Offspring_care);
            creatures_reproduction.Attributes.Add(creatures_reproduction_Mortality_rate);


            Category creatures_classification = new Category(8, "Classification", "<i class='material-icons' translate='no'>bubble_chart</i>", "classification");

            Attribute creatures_classification_Phylum = new Attribute("Phylum", "Phylum", "tinytext", "Write as little or as much as you want");
            Attribute creatures_classification_Class = new Attribute("Class", "Class", "tinytext", "Write as little or as much as you want");
            Attribute creatures_classification_Order = new Attribute("Order", "Order", "tinytext", "Write as little or as much as you want");
            Attribute creatures_classification_Family = new Attribute("Family", "Family", "tinytext", "Write as little or as much as you want");
            Attribute creatures_classification_Genus = new Attribute("Genus", "Genus", "tinytext", "Write as little or as much as you want");
            Attribute creatures_classification_Species = new Attribute("Species", "Species", "tinytext", "Write as little or as much as you want");
            Attribute creatures_classification_Variations = new Attribute("Variations", "Variations", "tinytext", "What variations of this creature are there across the world");
            creatures_classification.Attributes.Add(creatures_classification_Phylum);
            creatures_classification.Attributes.Add(creatures_classification_Class);
            creatures_classification.Attributes.Add(creatures_classification_Order);
            creatures_classification.Attributes.Add(creatures_classification_Family);
            creatures_classification.Attributes.Add(creatures_classification_Genus);
            creatures_classification.Attributes.Add(creatures_classification_Species);
            creatures_classification.Attributes.Add(creatures_classification_Variations);


            Category creatures_notes = new Category(10, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute creatures_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute creatures_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", "Write as little or as much as you want");
            creatures_notes.Attributes.Add(creatures_notes_Notes);
            creatures_notes.Attributes.Add(creatures_notes_Private_notes);

            Category creatures_gallery = new Category(9, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute creatures_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            creatures_gallery.Attributes.Add(creatures_gallery_gallery);


            content_creatures.categories.Add(creatures_overview);
            content_creatures.categories.Add(creatures_looks);
            content_creatures.categories.Add(creatures_traits);
            content_creatures.categories.Add(creatures_habitat);
            content_creatures.categories.Add(creatures_comparisons);
            content_creatures.categories.Add(creatures_evolution);
            content_creatures.categories.Add(creatures_reproduction);
            content_creatures.categories.Add(creatures_classification);
            content_creatures.categories.Add(creatures_notes);
            content_creatures.references.Add(creatures_gallery);


            content_creatures.categories = content_creatures.categories.OrderBy(c => c.Order).ToList();
            content_creatures.categories.AddAutoIncrmentId("Id");

            #endregion
            #region deities
            Content content_deities = new Content("deities", true);

            Category deities_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute deities_overview_Name = new Attribute("Name", "Name", "varchar", "What is this deitys name");
            Attribute deities_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this deity");
            Attribute deities_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this deity known by");
            Attribute deities_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this deity.", "universes");
            Attribute deities_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your deities.");
            deities_overview.Attributes.Add(deities_overview_Name);
            deities_overview.Attributes.Add(deities_overview_Description);
            deities_overview.Attributes.Add(deities_overview_Other_Names);
            deities_overview.Attributes.Add(deities_overview_Universe);
            deities_overview.Attributes.Add(deities_overview_Tags);


            Category deities_appearance = new Category(2, "Appearance", "<i class='material-icons' translate='no'>accessibility</i>", "appearance");

            Attribute deities_appearance_Physical_Description = new Attribute("Physical_Description", "Physical Description", "tinytext", "How would you describe what this deity looks like physically");
            Attribute deities_appearance_Height = new Attribute("Height", "Height", "double", "How tall is this deity");
            Attribute deities_appearance_Weight = new Attribute("Weight", "Weight", "double", "How much does this deity weigh");
            deities_appearance.Attributes.Add(deities_appearance_Physical_Description);
            deities_appearance.Attributes.Add(deities_appearance_Height);
            deities_appearance.Attributes.Add(deities_appearance_Weight);


            Category deities_family = new Category(3, "Family", "<i class='material-icons' translate='no'>supervisor_account</i>", "family");

            Attribute deities_family_Parents = new Attribute("Parents", "Parents", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_family_Partners = new Attribute("Partners", "Partners", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_family_Children = new Attribute("Children", "Children", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_family_Siblings = new Attribute("Siblings", "Siblings", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            deities_family.Attributes.Add(deities_family_Parents);
            deities_family.Attributes.Add(deities_family_Partners);
            deities_family.Attributes.Add(deities_family_Children);
            deities_family.Attributes.Add(deities_family_Siblings);


            Category deities_symbolism = new Category(4, "Symbolism", "<i class='material-icons' translate='no'>thumbs_up_down</i>", "symbolism");

            Attribute deities_symbolism_Symbols = new Attribute("Symbols", "Symbols", "tinytext", "What symbols are commonly associated with this deity");
            Attribute deities_symbolism_Elements = new Attribute("Elements", "Elements", "tinytext", "What elements are commonly associated with this deity");
            Attribute deities_symbolism_Creatures = new Attribute("Creatures", "Creatures", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_symbolism_Floras = new Attribute("Floras", "Floras", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_symbolism_Religions = new Attribute("Religions", "Religions", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_symbolism_Relics = new Attribute("Relics", "Relics", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            deities_symbolism.Attributes.Add(deities_symbolism_Symbols);
            deities_symbolism.Attributes.Add(deities_symbolism_Elements);
            deities_symbolism.Attributes.Add(deities_symbolism_Creatures);
            deities_symbolism.Attributes.Add(deities_symbolism_Floras);
            deities_symbolism.Attributes.Add(deities_symbolism_Religions);
            deities_symbolism.Attributes.Add(deities_symbolism_Relics);


            Category deities_powers = new Category(5, "Powers", "<i class='material-icons' translate='no'>grade</i>", "powers");

            Attribute deities_powers_Strengths = new Attribute("Strengths", "Strengths", "tinytext", "What are this deitys strengths");
            Attribute deities_powers_Weaknesses = new Attribute("Weaknesses", "Weaknesses", "tinytext", "What are this deitys weaknesses");
            Attribute deities_powers_Abilities = new Attribute("Abilities", "Abilities", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_powers_Conditions = new Attribute("Conditions", "Conditions", "tinytext", "What conditions does this deity have");
            deities_powers.Attributes.Add(deities_powers_Strengths);
            deities_powers.Attributes.Add(deities_powers_Weaknesses);
            deities_powers.Attributes.Add(deities_powers_Abilities);
            deities_powers.Attributes.Add(deities_powers_Conditions);


            Category deities_rituals = new Category(6, "Rituals", "<i class='material-icons' translate='no'>import_contacts</i>", "rituals");

            Attribute deities_rituals_Prayers = new Attribute("Prayers", "Prayers", "tinytext", "What prayers are commonly associated with this deity How do followers pray");
            Attribute deities_rituals_Rituals = new Attribute("Rituals", "Rituals", "tinytext", "What rituals are commonly associated with this deity How do they work");
            Attribute deities_rituals_Traditions = new Attribute("Traditions", "Traditions", "tinytext", "What traditions are commonly associated with this deity");
            Attribute deities_rituals_Human_Interaction = new Attribute("Human_Interaction", "Human Interaction", "tinytext", "How often does this deity interact with their followers In what ways");
            Attribute deities_rituals_Related_towns = new Attribute("Related_towns", "Related Towns", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_rituals_Related_races = new Attribute("Related_races", "Related Races", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            Attribute deities_rituals_Related_landmarks = new Attribute("Related_landmarks", "Related Landmarks", "tinytext", "This field allows you to link your other Notebook.ai pages to this deity.");
            deities_rituals.Attributes.Add(deities_rituals_Prayers);
            deities_rituals.Attributes.Add(deities_rituals_Rituals);
            deities_rituals.Attributes.Add(deities_rituals_Traditions);
            deities_rituals.Attributes.Add(deities_rituals_Human_Interaction);
            deities_rituals.Attributes.Add(deities_rituals_Related_towns);
            deities_rituals.Attributes.Add(deities_rituals_Related_races);
            deities_rituals.Attributes.Add(deities_rituals_Related_landmarks);


            Category deities_history = new Category(7, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute deities_history_Notable_Events = new Attribute("Notable_Events", "Notable Events", "tinytext", "What notable events throughout history has this deity been a part of");
            Attribute deities_history_Family_History = new Attribute("Family_History", "Family History", "tinytext", "What is this deitys family history");
            Attribute deities_history_Life_Story = new Attribute("Life_Story", "Life Story", "text", "What is this deitys life story");
            deities_history.Attributes.Add(deities_history_Notable_Events);
            deities_history.Attributes.Add(deities_history_Family_History);
            deities_history.Attributes.Add(deities_history_Life_Story);


            Category deities_notes = new Category(9, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute deities_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute deities_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            deities_notes.Attributes.Add(deities_notes_Notes);
            deities_notes.Attributes.Add(deities_notes_Private_Notes);

            Category deities_gallery = new Category(8, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute deities_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            deities_gallery.Attributes.Add(deities_gallery_gallery);


            content_deities.categories.Add(deities_overview);
            content_deities.categories.Add(deities_appearance);
            content_deities.categories.Add(deities_family);
            content_deities.categories.Add(deities_symbolism);
            content_deities.categories.Add(deities_powers);
            content_deities.categories.Add(deities_rituals);
            content_deities.categories.Add(deities_history);
            content_deities.categories.Add(deities_notes);
            content_deities.references.Add(deities_gallery);


            content_deities.categories = content_deities.categories.OrderBy(c => c.Order).ToList();
            content_deities.categories.AddAutoIncrmentId("Id");

            #endregion
            #region floras
            Content content_floras = new Content("floras", true);

            Category floras_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute floras_overview_Name = new Attribute("Name", "Name", "varchar", "Whats the name of this flora");
            Attribute floras_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this flora");
            Attribute floras_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this flora known by");
            Attribute floras_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this flora.", "universes");
            Attribute floras_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your floras.");
            floras_overview.Attributes.Add(floras_overview_Name);
            floras_overview.Attributes.Add(floras_overview_Description);
            floras_overview.Attributes.Add(floras_overview_Other_Names);
            floras_overview.Attributes.Add(floras_overview_Universe);
            floras_overview.Attributes.Add(floras_overview_Tags);


            Category floras_classification = new Category(2, "Classification", "<i class='material-icons' translate='no'>bubble_chart</i>", "classification");

            Attribute floras_classification_Order = new Attribute("Order", "Order", "varchar", "Write as little or as much as you want");
            Attribute floras_classification_Family = new Attribute("Family", "Family", "varchar", "Write as little or as much as you want");
            Attribute floras_classification_Genus = new Attribute("Genus", "Genus", "varchar", "Write as little or as much as you want");
            Attribute floras_classification_Related_flora = new Attribute("Related_flora", "Related Flora", "varchar", "This field allows you to link your other Notebook.ai pages to this flora.");
            floras_classification.Attributes.Add(floras_classification_Order);
            floras_classification.Attributes.Add(floras_classification_Family);
            floras_classification.Attributes.Add(floras_classification_Genus);
            floras_classification.Attributes.Add(floras_classification_Related_flora);


            Category floras_appearance = new Category(3, "Appearance", "<i class='material-icons' translate='no'>local_florist</i>", "appearance");

            Attribute floras_appearance_Size = new Attribute("Size", "Size", "varchar", "How big does this flora grow");
            Attribute floras_appearance_Smell = new Attribute("Smell", "Smell", "varchar", "What does this flora smell like");
            Attribute floras_appearance_Taste = new Attribute("Taste", "Taste", "varchar", "What does this flora taste like");
            Attribute floras_appearance_Colorings = new Attribute("Colorings", "Colorings", "varchar", "What kinds of colorings are found on each part of this flora");
            floras_appearance.Attributes.Add(floras_appearance_Size);
            floras_appearance.Attributes.Add(floras_appearance_Smell);
            floras_appearance.Attributes.Add(floras_appearance_Taste);
            floras_appearance.Attributes.Add(floras_appearance_Colorings);


            Category floras_produce = new Category(4, "Produce", "<i class='material-icons' translate='no'>add_box</i>", "produce");

            Attribute floras_produce_Fruits = new Attribute("Fruits", "Fruits", "varchar", "What fruits does this flora produce");
            Attribute floras_produce_Magical_effects = new Attribute("Magical_effects", "Magical Effects", "tinytext", "This field allows you to link your other Notebook.ai pages to this flora.");
            Attribute floras_produce_Material_uses = new Attribute("Material_uses", "Material Uses", "tinytext", "How is this flora used in producing other materials");
            Attribute floras_produce_Medicinal_purposes = new Attribute("Medicinal_purposes", "Medicinal Purposes", "tinytext", "Does this flora have any medicinal use");
            Attribute floras_produce_Berries = new Attribute("Berries", "Berries", "tinytext", "What kinds of berries does this flora produce");
            Attribute floras_produce_Nuts = new Attribute("Nuts", "Nuts", "varchar", "What kinds of nuts does this flora produce");
            Attribute floras_produce_Seeds = new Attribute("Seeds", "Seeds", "varchar", "What kinds of seeds does this flora produce");
            floras_produce.Attributes.Add(floras_produce_Fruits);
            floras_produce.Attributes.Add(floras_produce_Magical_effects);
            floras_produce.Attributes.Add(floras_produce_Material_uses);
            floras_produce.Attributes.Add(floras_produce_Medicinal_purposes);
            floras_produce.Attributes.Add(floras_produce_Berries);
            floras_produce.Attributes.Add(floras_produce_Nuts);
            floras_produce.Attributes.Add(floras_produce_Seeds);


            Category floras_ecosystem = new Category(5, "Ecosystem", "<i class='material-icons' translate='no'>language</i>", "ecosystem");

            Attribute floras_ecosystem_Locations = new Attribute("Locations", "Locations", "tinytext", "This field allows you to link your other Notebook.ai pages to this flora.");
            Attribute floras_ecosystem_Reproduction = new Attribute("Reproduction", "Reproduction", "tinytext", "How does this flora reproduce and spread");
            Attribute floras_ecosystem_Seasonality = new Attribute("Seasonality", "Seasonality", "tinytext", "What seasons or climates is this flora most often found in");
            Attribute floras_ecosystem_Eaten_by = new Attribute("Eaten_by", "Eaten By", "tinytext", "This field allows you to link your other Notebook.ai pages to this flora.");
            floras_ecosystem.Attributes.Add(floras_ecosystem_Locations);
            floras_ecosystem.Attributes.Add(floras_ecosystem_Reproduction);
            floras_ecosystem.Attributes.Add(floras_ecosystem_Seasonality);
            floras_ecosystem.Attributes.Add(floras_ecosystem_Eaten_by);


            Category floras_notes = new Category(7, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute floras_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute floras_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            floras_notes.Attributes.Add(floras_notes_Notes);
            floras_notes.Attributes.Add(floras_notes_Private_Notes);

            Category floras_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute floras_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            floras_gallery.Attributes.Add(floras_gallery_gallery);


            content_floras.categories.Add(floras_overview);
            content_floras.categories.Add(floras_classification);
            content_floras.categories.Add(floras_appearance);
            content_floras.categories.Add(floras_produce);
            content_floras.categories.Add(floras_ecosystem);
            content_floras.categories.Add(floras_notes);
            content_floras.references.Add(floras_gallery);


            content_floras.categories = content_floras.categories.OrderBy(c => c.Order).ToList();
            content_floras.categories.AddAutoIncrmentId("Id");

            #endregion
            #region foods
            Content content_foods = new Content("foods", true);

            Category foods_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute foods_overview_Name = new Attribute("Name", "Name", "varchar", "What is the name of this food");
            Attribute foods_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this food");
            Attribute foods_overview_Type_of_food = new Attribute("Type_of_food", "Type Of Food", "text", "What kind of food is this food");
            Attribute foods_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this food also known by");
            Attribute foods_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this food.", "universes");
            Attribute foods_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your foods.");
            foods_overview.Attributes.Add(foods_overview_Name);
            foods_overview.Attributes.Add(foods_overview_Description);
            foods_overview.Attributes.Add(foods_overview_Type_of_food);
            foods_overview.Attributes.Add(foods_overview_Other_Names);
            foods_overview.Attributes.Add(foods_overview_Universe);
            foods_overview.Attributes.Add(foods_overview_Tags);


            Category foods_recipe = new Category(2, "Recipe", "<i class='material-icons' translate='no'>kitchen</i>", "recipe");

            Attribute foods_recipe_Ingredients = new Attribute("Ingredients", "Ingredients", "tinytext", "What ingredients are in this food");
            Attribute foods_recipe_Preparation = new Attribute("Preparation", "Preparation", "tinytext", "How do you prepare this food");
            Attribute foods_recipe_Cooking_method = new Attribute("Cooking_method", "Cooking Method", "tinytext", "How do you cook this food");
            Attribute foods_recipe_Spices = new Attribute("Spices", "Spices", "tinytext", "What spices are common in this food");
            Attribute foods_recipe_Yield = new Attribute("Yield", "Yield", "tinytext", "How much this food is usually made at a time");
            Attribute foods_recipe_Color = new Attribute("Color", "Color", "varchar", "What colors does this food come in");
            Attribute foods_recipe_Size = new Attribute("Size", "Size", "double", "How big is this food");
            Attribute foods_recipe_Variations = new Attribute("Variations", "Variations", "tinytext", "What are the most common variants of this food");
            Attribute foods_recipe_Smell = new Attribute("Smell", "Smell", "varchar", "What does this food smell like when cooking");
            foods_recipe.Attributes.Add(foods_recipe_Ingredients);
            foods_recipe.Attributes.Add(foods_recipe_Preparation);
            foods_recipe.Attributes.Add(foods_recipe_Cooking_method);
            foods_recipe.Attributes.Add(foods_recipe_Spices);
            foods_recipe.Attributes.Add(foods_recipe_Yield);
            foods_recipe.Attributes.Add(foods_recipe_Color);
            foods_recipe.Attributes.Add(foods_recipe_Size);
            foods_recipe.Attributes.Add(foods_recipe_Variations);
            foods_recipe.Attributes.Add(foods_recipe_Smell);


            Category foods_sales = new Category(3, "Sales", "<i class='material-icons' translate='no'>room_service</i>", "sales");

            Attribute foods_sales_Cost = new Attribute("Cost", "Cost", "tinytext", "How much does this food cost");
            Attribute foods_sales_Sold_by = new Attribute("Sold_by", "Sold By", "tinytext", "Who sells this food");
            Attribute foods_sales_Rarity = new Attribute("Rarity", "Rarity", "tinytext", "How rare is it to see this food");
            Attribute foods_sales_Shelf_life = new Attribute("Shelf_life", "Shelf Life", "tinytext", "How long does it take for this food to go bad");
            foods_sales.Attributes.Add(foods_sales_Cost);
            foods_sales.Attributes.Add(foods_sales_Sold_by);
            foods_sales.Attributes.Add(foods_sales_Rarity);
            foods_sales.Attributes.Add(foods_sales_Shelf_life);


            Category foods_eating = new Category(4, "Eating", "<i class='material-icons' translate='no'>restaurant</i>", "eating");

            Attribute foods_eating_Meal = new Attribute("Meal", "Meal", "tinytext", "Which meal of the day is this food usually served at");
            Attribute foods_eating_Serving = new Attribute("Serving", "Serving", "tinytext", "How is this food served");
            Attribute foods_eating_Utensils_needed = new Attribute("Utensils_needed", "Utensils Needed", "tinytext", "What utensils are needed to eat this food");
            Attribute foods_eating_Texture = new Attribute("Texture", "Texture", "tinytext", "What is the texture of this food");
            Attribute foods_eating_Scent = new Attribute("Scent", "Scent", "tinytext", "What does this food smell like when served");
            Attribute foods_eating_Flavor = new Attribute("Flavor", "Flavor", "tinytext", "What are this foods flavors");
            foods_eating.Attributes.Add(foods_eating_Meal);
            foods_eating.Attributes.Add(foods_eating_Serving);
            foods_eating.Attributes.Add(foods_eating_Utensils_needed);
            foods_eating.Attributes.Add(foods_eating_Texture);
            foods_eating.Attributes.Add(foods_eating_Scent);
            foods_eating.Attributes.Add(foods_eating_Flavor);


            Category foods_effects = new Category(5, "Effects", "<i class='material-icons' translate='no'>cake</i>", "effects");

            Attribute foods_effects_Nutrition = new Attribute("Nutrition", "Nutrition", "tinytext", "How nutritious is this food What nutrients does it have");
            Attribute foods_effects_Conditions = new Attribute("Conditions", "Conditions", "tinytext", "What conditons can this food cause");
            Attribute foods_effects_Side_effects = new Attribute("Side_effects", "Side Effects", "tinytext", "What are the common side effects of eating this food");
            foods_effects.Attributes.Add(foods_effects_Nutrition);
            foods_effects.Attributes.Add(foods_effects_Conditions);
            foods_effects.Attributes.Add(foods_effects_Side_effects);


            Category foods_history = new Category(6, "History", "<i class='material-icons' translate='no'>history</i>", "history");

            Attribute foods_history_Place_of_origin = new Attribute("Place_of_origin", "Place Of Origin", "tinytext", "Where did this food originate");
            Attribute foods_history_Origin_story = new Attribute("Origin_story", "Origin Story", "tinytext", "What is the origin story of this food");
            Attribute foods_history_Traditions = new Attribute("Traditions", "Traditions", "tinytext", "What traditions is this food commonly associated with or eaten during");
            Attribute foods_history_Symbolisms = new Attribute("Symbolisms", "Symbolisms", "tinytext", "What does this food symbolize in your world");
            Attribute foods_history_Related_foods = new Attribute("Related_foods", "Related Foods", "tinytext", "What other foods are related to this food");
            Attribute foods_history_Reputation = new Attribute("Reputation", "Reputation", "tinytext", "What reputation does this food have");
            foods_history.Attributes.Add(foods_history_Place_of_origin);
            foods_history.Attributes.Add(foods_history_Origin_story);
            foods_history.Attributes.Add(foods_history_Traditions);
            foods_history.Attributes.Add(foods_history_Symbolisms);
            foods_history.Attributes.Add(foods_history_Related_foods);
            foods_history.Attributes.Add(foods_history_Reputation);


            Category foods_changelog = new Category(8, "Changelog", "<i class='material-icons' translate='no'>history</i>", "changelog");



            Category foods_notes = new Category(9, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute foods_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute foods_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            foods_notes.Attributes.Add(foods_notes_Notes);
            foods_notes.Attributes.Add(foods_notes_Private_Notes);

            Category foods_gallery = new Category(7, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute foods_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            foods_gallery.Attributes.Add(foods_gallery_gallery);


            content_foods.categories.Add(foods_overview);
            content_foods.categories.Add(foods_recipe);
            content_foods.categories.Add(foods_sales);
            content_foods.categories.Add(foods_eating);
            content_foods.categories.Add(foods_effects);
            content_foods.categories.Add(foods_history);
            content_foods.categories.Add(foods_changelog);
            content_foods.categories.Add(foods_notes);
            content_foods.references.Add(foods_gallery);


            content_foods.categories = content_foods.categories.OrderBy(c => c.Order).ToList();
            content_foods.categories.AddAutoIncrmentId("Id");

            #endregion
            #region governments
            Content content_governments = new Content("governments", true);

            Category governments_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute governments_overview_Name = new Attribute("Name", "Name", "varchar", "Whats the name of this government");
            Attribute governments_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this government");
            Attribute governments_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this government.", "universes");
            Attribute governments_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your governments.");
            governments_overview.Attributes.Add(governments_overview_Name);
            governments_overview.Attributes.Add(governments_overview_Description);
            governments_overview.Attributes.Add(governments_overview_Universe);
            governments_overview.Attributes.Add(governments_overview_Tags);


            Category governments_structure = new Category(2, "Structure", "<i class='material-icons' translate='no'>view_list</i>", "structure");

            Attribute governments_structure_Type_Of_Government = new Attribute("Type_Of_Government", "Type Of Government", "varchar", "What type of government is this government");
            Attribute governments_structure_Power_Structure = new Attribute("Power_Structure", "Power Structure", "tinytext", "How is this governments power structured How is it organized Who has what powers");
            Attribute governments_structure_Power_Source = new Attribute("Power_Source", "Power Source", "varchar", "Where does this governments source of power come from");
            Attribute governments_structure_Checks_And_Balances = new Attribute("Checks_And_Balances", "Checks And Balances", "tinytext", "What checks and balances does this government have in place");
            Attribute governments_structure_Jobs = new Attribute("Jobs", "Jobs", "tinytext", "What jobs does this government provide What jobs are necessary to run it");
            governments_structure.Attributes.Add(governments_structure_Type_Of_Government);
            governments_structure.Attributes.Add(governments_structure_Power_Structure);
            governments_structure.Attributes.Add(governments_structure_Power_Source);
            governments_structure.Attributes.Add(governments_structure_Checks_And_Balances);
            governments_structure.Attributes.Add(governments_structure_Jobs);


            Category governments_ideologies = new Category(3, "Ideologies", "<i class='material-icons' translate='no'>cloud</i>", "ideologies");

            Attribute governments_ideologies_Sociopolitical = new Attribute("Sociopolitical", "Sociopolitical", "tinytext", "What sociopolitical ideologies does this government hold");
            Attribute governments_ideologies_Socioeconomical = new Attribute("Socioeconomical", "Socioeconomical", "tinytext", "What socioeconomical ideologies does this government hold");
            Attribute governments_ideologies_Geocultural = new Attribute("Geocultural", "Geocultural", "tinytext", "What geocultural ideologies does this government hold");
            Attribute governments_ideologies_Laws = new Attribute("Laws", "Laws", "tinytext", "What are the laws of this government");
            Attribute governments_ideologies_Immigration = new Attribute("Immigration", "Immigration", "tinytext", "What immigration policies and ideologies does this government hold");
            Attribute governments_ideologies_Privacy_Ideologies = new Attribute("Privacy_Ideologies", "Privacy Ideologies", "tinytext", "What does this government think about privacy");
            governments_ideologies.Attributes.Add(governments_ideologies_Sociopolitical);
            governments_ideologies.Attributes.Add(governments_ideologies_Socioeconomical);
            governments_ideologies.Attributes.Add(governments_ideologies_Geocultural);
            governments_ideologies.Attributes.Add(governments_ideologies_Laws);
            governments_ideologies.Attributes.Add(governments_ideologies_Immigration);
            governments_ideologies.Attributes.Add(governments_ideologies_Privacy_Ideologies);


            Category governments_process = new Category(4, "Process", "<i class='material-icons' translate='no'>gavel</i>", "process");

            Attribute governments_process_Electoral_Process = new Attribute("Electoral_Process", "Electoral Process", "tinytext", "What is the electoral process of this government");
            Attribute governments_process_Term_Lengths = new Attribute("Term_Lengths", "Term Lengths", "tinytext", "What are the term lengths for people in this government");
            Attribute governments_process_Criminal_System = new Attribute("Criminal_System", "Criminal System", "tinytext", "What is this governments criminal system like");
            governments_process.Attributes.Add(governments_process_Electoral_Process);
            governments_process.Attributes.Add(governments_process_Term_Lengths);
            governments_process.Attributes.Add(governments_process_Criminal_System);


            Category governments_populace = new Category(5, "Populace", "<i class='material-icons' translate='no'>visibility</i>", "populace");

            Attribute governments_populace_Approval_Ratings = new Attribute("Approval_Ratings", "Approval Ratings", "tinytext", "What do the people think of this government What are their approval ratings");
            Attribute governments_populace_International_Relations = new Attribute("International_Relations", "International Relations", "tinytext", "What do international governments think about this government How are their relations");
            Attribute governments_populace_Civilian_Life = new Attribute("Civilian_Life", "Civilian Life", "tinytext", "What is civilian life like for the people ruled by this government");
            governments_populace.Attributes.Add(governments_populace_Approval_Ratings);
            governments_populace.Attributes.Add(governments_populace_International_Relations);
            governments_populace.Attributes.Add(governments_populace_Civilian_Life);


            Category governments_members = new Category(6, "Members", "<i class='material-icons' translate='no'>group</i>", "members");

            Attribute governments_members_Leaders = new Attribute("Leaders", "Leaders", "tinytext", "This field allows you to link your other Notebook.ai pages to this government.");
            Attribute governments_members_Groups = new Attribute("Groups", "Groups", "tinytext", "This field allows you to link your other Notebook.ai pages to this government.");
            Attribute governments_members_Political_figures = new Attribute("Political_figures", "Political Figures", "tinytext", "This field allows you to link your other Notebook.ai pages to this government.");
            Attribute governments_members_Military = new Attribute("Military", "Military", "tinytext", "What does this governments military look like");
            Attribute governments_members_Navy = new Attribute("Navy", "Navy", "tinytext", "What does this governments navy look like");
            Attribute governments_members_Airforce = new Attribute("Airforce", "Airforce", "tinytext", "What does this governments airforce look like");
            Attribute governments_members_Space_Program = new Attribute("Space_Program", "Space Program", "tinytext", "Does this government have a space program What is it like");
            governments_members.Attributes.Add(governments_members_Leaders);
            governments_members.Attributes.Add(governments_members_Groups);
            governments_members.Attributes.Add(governments_members_Political_figures);
            governments_members.Attributes.Add(governments_members_Military);
            governments_members.Attributes.Add(governments_members_Navy);
            governments_members.Attributes.Add(governments_members_Airforce);
            governments_members.Attributes.Add(governments_members_Space_Program);


            Category governments_history = new Category(7, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute governments_history_Founding_Story = new Attribute("Founding_Story", "Founding Story", "tinytext", "How was this government created What is its founding story");
            Attribute governments_history_Flag_Design_Story = new Attribute("Flag_Design_Story", "Flag Design Story", "tinytext", "How was this governments flag designed");
            Attribute governments_history_Notable_Wars = new Attribute("Notable_Wars", "Notable Wars", "tinytext", "What notable wars throughout history has this government been involved in");
            Attribute governments_history_Holidays = new Attribute("Holidays", "Holidays", "tinytext", "What holidays are relevant to this governments history What holidays does it recognize");
            governments_history.Attributes.Add(governments_history_Founding_Story);
            governments_history.Attributes.Add(governments_history_Flag_Design_Story);
            governments_history.Attributes.Add(governments_history_Notable_Wars);
            governments_history.Attributes.Add(governments_history_Holidays);


            Category governments_assets = new Category(8, "Assets", "<i class='material-icons' translate='no'>shopping_cart</i>", "assets");

            Attribute governments_assets_Items = new Attribute("Items", "Items", "tinytext", "This field allows you to link your other Notebook.ai pages to this government.");
            Attribute governments_assets_Technologies = new Attribute("Technologies", "Technologies", "tinytext", "This field allows you to link your other Notebook.ai pages to this government.");
            Attribute governments_assets_Creatures = new Attribute("Creatures", "Creatures", "tinytext", "This field allows you to link your other Notebook.ai pages to this government.");
            Attribute governments_assets_Vehicles = new Attribute("Vehicles", "Vehicles", "tinytext", "What vehicles does this government use or own");
            governments_assets.Attributes.Add(governments_assets_Items);
            governments_assets.Attributes.Add(governments_assets_Technologies);
            governments_assets.Attributes.Add(governments_assets_Creatures);
            governments_assets.Attributes.Add(governments_assets_Vehicles);


            Category governments_notes = new Category(10, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute governments_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute governments_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            governments_notes.Attributes.Add(governments_notes_Notes);
            governments_notes.Attributes.Add(governments_notes_Private_Notes);

            Category governments_gallery = new Category(9, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute governments_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            governments_gallery.Attributes.Add(governments_gallery_gallery);


            content_governments.categories.Add(governments_overview);
            content_governments.categories.Add(governments_structure);
            content_governments.categories.Add(governments_ideologies);
            content_governments.categories.Add(governments_process);
            content_governments.categories.Add(governments_populace);
            content_governments.categories.Add(governments_members);
            content_governments.categories.Add(governments_history);
            content_governments.categories.Add(governments_assets);
            content_governments.categories.Add(governments_notes);
            content_governments.references.Add(governments_gallery);


            content_governments.categories = content_governments.categories.OrderBy(c => c.Order).ToList();
            content_governments.categories.AddAutoIncrmentId("Id");

            #endregion
            #region groups
            Content content_groups = new Content("groups", true);

            Category groups_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute groups_overview_Name = new Attribute("Name", "Name", "varchar", "What is the name of this group");
            Attribute groups_overview_Description = new Attribute("Description", "Description", "text", "How would you describe the this group group");
            Attribute groups_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this group known by");
            Attribute groups_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this group.", "universes");
            Attribute groups_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your groups.");
            groups_overview.Attributes.Add(groups_overview_Name);
            groups_overview.Attributes.Add(groups_overview_Description);
            groups_overview.Attributes.Add(groups_overview_Other_Names);
            groups_overview.Attributes.Add(groups_overview_Universe);
            groups_overview.Attributes.Add(groups_overview_Tags);


            Category groups_members = new Category(2, "Members", "<i class='material-icons' translate='no'>list</i>", "members");

            Attribute groups_members_Leaders = new Attribute("Leaders", "Leaders", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_members_Creatures = new Attribute("Creatures", "Creatures", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_members_Members = new Attribute("Members", "Members", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            groups_members.Attributes.Add(groups_members_Leaders);
            groups_members.Attributes.Add(groups_members_Creatures);
            groups_members.Attributes.Add(groups_members_Members);


            Category groups_locations = new Category(3, "Locations", "<i class='material-icons' translate='no'>location_on</i>", "locations");

            Attribute groups_locations_Locations = new Attribute("Locations", "Locations", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_locations_Headquarters = new Attribute("Headquarters", "Headquarters", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_locations_Offices = new Attribute("Offices", "Offices", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            groups_locations.Attributes.Add(groups_locations_Locations);
            groups_locations.Attributes.Add(groups_locations_Headquarters);
            groups_locations.Attributes.Add(groups_locations_Offices);


            Category groups_purpose = new Category(4, "Purpose", "<i class='material-icons' translate='no'>business</i>", "purpose");

            Attribute groups_purpose_Motivations = new Attribute("Motivations", "Motivations", "varchar", "What motivates this group");
            Attribute groups_purpose_Goals = new Attribute("Goals", "Goals", "varchar", "What is the primary goal of this group");
            Attribute groups_purpose_Obstacles = new Attribute("Obstacles", "Obstacles", "varchar", "What obstacles stand in the way of this group");
            Attribute groups_purpose_Risks = new Attribute("Risks", "Risks", "varchar", "What risks are on the line for this group");
            Attribute groups_purpose_Traditions = new Attribute("Traditions", "Traditions", "varchar", "What traditions does this group partake in");
            groups_purpose.Attributes.Add(groups_purpose_Motivations);
            groups_purpose.Attributes.Add(groups_purpose_Goals);
            groups_purpose.Attributes.Add(groups_purpose_Obstacles);
            groups_purpose.Attributes.Add(groups_purpose_Risks);
            groups_purpose.Attributes.Add(groups_purpose_Traditions);


            Category groups_politics = new Category(5, "Politics", "<i class='material-icons' translate='no'>thumbs_up_down</i>", "politics");

            Attribute groups_politics_Allies = new Attribute("Allies", "Allies", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_politics_Enemies = new Attribute("Enemies", "Enemies", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_politics_Rivals = new Attribute("Rivals", "Rivals", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_politics_Clients = new Attribute("Clients", "Clients", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            groups_politics.Attributes.Add(groups_politics_Allies);
            groups_politics.Attributes.Add(groups_politics_Enemies);
            groups_politics.Attributes.Add(groups_politics_Rivals);
            groups_politics.Attributes.Add(groups_politics_Clients);


            Category groups_inventory = new Category(6, "Inventory", "<i class='material-icons' translate='no'>shopping_cart</i>", "inventory");

            Attribute groups_inventory_Inventory = new Attribute("Inventory", "Inventory", "varchar", "What kinds of items does this group keep in inventory");
            Attribute groups_inventory_Equipment = new Attribute("Equipment", "Equipment", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_inventory_Key_items = new Attribute("Key_items", "Key Items", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_inventory_Suppliers = new Attribute("Suppliers", "Suppliers", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            groups_inventory.Attributes.Add(groups_inventory_Inventory);
            groups_inventory.Attributes.Add(groups_inventory_Equipment);
            groups_inventory.Attributes.Add(groups_inventory_Key_items);
            groups_inventory.Attributes.Add(groups_inventory_Suppliers);


            Category groups_hierarchy = new Category(7, "Hierarchy", "<i class='material-icons' translate='no'>call_split</i>", "hierarchy");

            Attribute groups_hierarchy_Supergroups = new Attribute("Supergroups", "Supergroups", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_hierarchy_Subgroups = new Attribute("Subgroups", "Subgroups", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_hierarchy_Sistergroups = new Attribute("Sistergroups", "Sistergroups", "varchar", "This field allows you to link your other Notebook.ai pages to this group.");
            Attribute groups_hierarchy_Organization_structure = new Attribute("Organization_structure", "Organization Structure", "tinytext", "Write as little or as much as you want");
            groups_hierarchy.Attributes.Add(groups_hierarchy_Supergroups);
            groups_hierarchy.Attributes.Add(groups_hierarchy_Subgroups);
            groups_hierarchy.Attributes.Add(groups_hierarchy_Sistergroups);
            groups_hierarchy.Attributes.Add(groups_hierarchy_Organization_structure);


            Category groups_notes = new Category(9, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute groups_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute groups_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", "Write as little or as much as you want");
            groups_notes.Attributes.Add(groups_notes_Notes);
            groups_notes.Attributes.Add(groups_notes_Private_notes);

            Category groups_gallery = new Category(8, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute groups_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            groups_gallery.Attributes.Add(groups_gallery_gallery);


            content_groups.categories.Add(groups_overview);
            content_groups.categories.Add(groups_members);
            content_groups.categories.Add(groups_locations);
            content_groups.categories.Add(groups_purpose);
            content_groups.categories.Add(groups_politics);
            content_groups.categories.Add(groups_inventory);
            content_groups.categories.Add(groups_hierarchy);
            content_groups.categories.Add(groups_notes);
            content_groups.references.Add(groups_gallery);


            content_groups.categories = content_groups.categories.OrderBy(c => c.Order).ToList();
            content_groups.categories.AddAutoIncrmentId("Id");

            #endregion
            #region items
            Content content_items = new Content("items", true);

            Category items_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute items_overview_Name = new Attribute("Name", "Name", "varchar", "What is this items full name");
            Attribute items_overview_Item_Type = new Attribute("Item_Type", "Item Type", "varchar", "What type of item is this item");
            Attribute items_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this item.", "universes");
            Attribute items_overview_Description = new Attribute("Description", "Description", "text", "Describe this item.");
            Attribute items_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your items.");
            Attribute items_overview_Technical_effects = new Attribute("Technical_effects", "Technical Effects", "tinytext", "");
            Attribute items_overview_Technology = new Attribute("Technology", "Technology", "varchar", "");
            items_overview.Attributes.Add(items_overview_Name);
            items_overview.Attributes.Add(items_overview_Item_Type);
            items_overview.Attributes.Add(items_overview_Universe);
            items_overview.Attributes.Add(items_overview_Description);
            items_overview.Attributes.Add(items_overview_Tags);
            items_overview.Attributes.Add(items_overview_Technical_effects);
            items_overview.Attributes.Add(items_overview_Technology);


            Category items_looks = new Category(2, "Looks", "<i class='material-icons' translate='no'>redeem</i>", "looks");

            Attribute items_looks_Materials = new Attribute("Materials", "Materials", "varchar", "What is this item made out of");
            Attribute items_looks_Weight = new Attribute("Weight", "Weight", "double", "How much does this item weigh");
            items_looks.Attributes.Add(items_looks_Materials);
            items_looks.Attributes.Add(items_looks_Weight);


            Category items_history = new Category(3, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute items_history_Original_Owners = new Attribute("Original_Owners", "Original Owners", "varchar", "This field allows you to link your other Notebook.ai pages to this item.");
            Attribute items_history_Past_Owners = new Attribute("Past_Owners", "Past Owners", "tinytext", "This field allows you to link your other Notebook.ai pages to this item.");
            Attribute items_history_Current_Owners = new Attribute("Current_Owners", "Current Owners", "varchar", "This field allows you to link your other Notebook.ai pages to this item.");
            Attribute items_history_Makers = new Attribute("Makers", "Makers", "varchar", "This field allows you to link your other Notebook.ai pages to this item.");
            Attribute items_history_Year_it_was_made = new Attribute("Year_it_was_made", "Year It Was Made", "int", "When was this item made");
            items_history.Attributes.Add(items_history_Original_Owners);
            items_history.Attributes.Add(items_history_Past_Owners);
            items_history.Attributes.Add(items_history_Current_Owners);
            items_history.Attributes.Add(items_history_Makers);
            items_history.Attributes.Add(items_history_Year_it_was_made);


            Category items_abilities = new Category(4, "Abilities", "<i class='material-icons' translate='no'>flash_on</i>", "abilities");

            Attribute items_abilities_Magical_effects = new Attribute("Magical_effects", "Magical Effects", "tinytext", "What kind of magic does this item possess");
            Attribute items_abilities_Magic = new Attribute("Magic", "Magic", "varchar", "This field allows you to link your other Notebook.ai pages to this item.");
            items_abilities.Attributes.Add(items_abilities_Magical_effects);
            items_abilities.Attributes.Add(items_abilities_Magic);


            Category items_notes = new Category(6, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute items_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute items_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            items_notes.Attributes.Add(items_notes_Notes);
            items_notes.Attributes.Add(items_notes_Private_Notes);

            Category items_gallery = new Category(5, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute items_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            items_gallery.Attributes.Add(items_gallery_gallery);


            content_items.categories.Add(items_overview);
            content_items.categories.Add(items_looks);
            content_items.categories.Add(items_history);
            content_items.categories.Add(items_abilities);
            content_items.categories.Add(items_notes);
            content_items.references.Add(items_gallery);


            content_items.categories = content_items.categories.OrderBy(c => c.Order).ToList();
            content_items.categories.AddAutoIncrmentId("Id");

            #endregion
            #region jobs
            Content content_jobs = new Content("jobs", true);

            Category jobs_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute jobs_overview_Name = new Attribute("Name", "Name", "varchar", "What is the name of this job");
            Attribute jobs_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this job.", "universes");
            Attribute jobs_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this job");
            Attribute jobs_overview_Type_of_job = new Attribute("Type_of_job", "Type Of Job", "varchar", "What kind of job is this job");
            Attribute jobs_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "text", "What other names is this job referred to as");
            Attribute jobs_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your jobs.");
            jobs_overview.Attributes.Add(jobs_overview_Name);
            jobs_overview.Attributes.Add(jobs_overview_Universe);
            jobs_overview.Attributes.Add(jobs_overview_Description);
            jobs_overview.Attributes.Add(jobs_overview_Type_of_job);
            jobs_overview.Attributes.Add(jobs_overview_Alternate_names);
            jobs_overview.Attributes.Add(jobs_overview_Tags);


            Category jobs_requirements = new Category(2, "Requirements", "<i class='material-icons' translate='no'>storage</i>", "requirements");

            Attribute jobs_requirements_Education = new Attribute("Education", "Education", "varchar", "What kind of education is necessary to work for this job");
            Attribute jobs_requirements_Experience = new Attribute("Experience", "Experience", "varchar", "What experience is necessary to work for this job");
            Attribute jobs_requirements_Training = new Attribute("Training", "Training", "varchar", "What training is necessary to work for this job");
            Attribute jobs_requirements_Work_hours = new Attribute("Work_hours", "Work Hours", "double", "How long are shifts at this job When do people work");
            Attribute jobs_requirements_Vehicles = new Attribute("Vehicles", "Vehicles", "varchar", "What vehicles does this job use");
            jobs_requirements.Attributes.Add(jobs_requirements_Education);
            jobs_requirements.Attributes.Add(jobs_requirements_Experience);
            jobs_requirements.Attributes.Add(jobs_requirements_Training);
            jobs_requirements.Attributes.Add(jobs_requirements_Work_hours);
            jobs_requirements.Attributes.Add(jobs_requirements_Vehicles);


            Category jobs_risks = new Category(3, "Risks", "<i class='material-icons' translate='no'>hdr_weak</i>", "risks");

            Attribute jobs_risks_Occupational_hazards = new Attribute("Occupational_hazards", "Occupational Hazards", "varchar", "What are the most common risks and hazards associated with the this job job");
            Attribute jobs_risks_Long_term_risks = new Attribute("Long_term_risks", "Long Term Risks", "varchar", "");
            jobs_risks.Attributes.Add(jobs_risks_Occupational_hazards);
            jobs_risks.Attributes.Add(jobs_risks_Long_term_risks);


            Category jobs_rewards = new Category(4, "Rewards", "<i class='material-icons' translate='no'>hdr_strong</i>", "rewards");

            Attribute jobs_rewards_Pay_rate = new Attribute("Pay_rate", "Pay Rate", "double", "How much does this job pay");
            Attribute jobs_rewards_Time_off = new Attribute("Time_off", "Time Off", "varchar", "How much time off do workers of this job get");
            jobs_rewards.Attributes.Add(jobs_rewards_Pay_rate);
            jobs_rewards.Attributes.Add(jobs_rewards_Time_off);


            Category jobs_specialization = new Category(5, "Specialization", "<i class='material-icons' translate='no'>grain</i>", "specialization");

            Attribute jobs_specialization_Promotions = new Attribute("Promotions", "Promotions", "varchar", "How are promotions handled at this job");
            Attribute jobs_specialization_Specializations = new Attribute("Specializations", "Specializations", "tinytext", "How can someone working at this job specialize");
            Attribute jobs_specialization_Field = new Attribute("Field", "Field", "tinytext", "What field is this job in");
            Attribute jobs_specialization_Ranks = new Attribute("Ranks", "Ranks", "tinytext", "Whats the hierarchy at this job What ranks are available");
            Attribute jobs_specialization_Similar_jobs = new Attribute("Similar_jobs", "Similar Jobs", "varchar", "What other jobs are similar to this job");
            jobs_specialization.Attributes.Add(jobs_specialization_Promotions);
            jobs_specialization.Attributes.Add(jobs_specialization_Specializations);
            jobs_specialization.Attributes.Add(jobs_specialization_Field);
            jobs_specialization.Attributes.Add(jobs_specialization_Ranks);
            jobs_specialization.Attributes.Add(jobs_specialization_Similar_jobs);


            Category jobs_history = new Category(6, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute jobs_history_Job_origin = new Attribute("Job_origin", "Job Origin", "tinytext", "How did this job originate");
            Attribute jobs_history_Initial_goal = new Attribute("Initial_goal", "Initial Goal", "varchar", "What was the original goal of this job");
            Attribute jobs_history_Notable_figures = new Attribute("Notable_figures", "Notable Figures", "varchar", "What notable figures are related to this job");
            Attribute jobs_history_Traditions = new Attribute("Traditions", "Traditions", "tinytext", "What traditions are related to this job");
            jobs_history.Attributes.Add(jobs_history_Job_origin);
            jobs_history.Attributes.Add(jobs_history_Initial_goal);
            jobs_history.Attributes.Add(jobs_history_Notable_figures);
            jobs_history.Attributes.Add(jobs_history_Traditions);


            Category jobs_notes = new Category(8, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute jobs_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute jobs_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            jobs_notes.Attributes.Add(jobs_notes_Notes);
            jobs_notes.Attributes.Add(jobs_notes_Private_Notes);

            Category jobs_gallery = new Category(7, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute jobs_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            jobs_gallery.Attributes.Add(jobs_gallery_gallery);


            content_jobs.categories.Add(jobs_overview);
            content_jobs.categories.Add(jobs_requirements);
            content_jobs.categories.Add(jobs_risks);
            content_jobs.categories.Add(jobs_rewards);
            content_jobs.categories.Add(jobs_specialization);
            content_jobs.categories.Add(jobs_history);
            content_jobs.categories.Add(jobs_notes);
            content_jobs.references.Add(jobs_gallery);


            content_jobs.categories = content_jobs.categories.OrderBy(c => c.Order).ToList();
            content_jobs.categories.AddAutoIncrmentId("Id");

            #endregion
            #region landmarks
            Content content_landmarks = new Content("landmarks", true);

            Category landmarks_ecosystem = new Category(1, "Ecosystem", "<i class='material-icons' translate='no'>local_florist</i>", "ecosystem");

            Attribute landmarks_ecosystem_Flora = new Attribute("Flora", "Flora", "varchar", "This field allows you to link your other Notebook.ai pages to this landmark.");
            Attribute landmarks_ecosystem_Creatures = new Attribute("Creatures", "Creatures", "varchar", "This field allows you to link your other Notebook.ai pages to this landmark.");
            landmarks_ecosystem.Attributes.Add(landmarks_ecosystem_Flora);
            landmarks_ecosystem.Attributes.Add(landmarks_ecosystem_Creatures);


            Category landmarks_history = new Category(1, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute landmarks_history_Creation_story = new Attribute("Creation_story", "Creation Story", "tinytext", "How did this landmark originally form");
            Attribute landmarks_history_Established_year = new Attribute("Established_year", "Established Year", "int", "When did this landmark originate");
            landmarks_history.Attributes.Add(landmarks_history_Creation_story);
            landmarks_history.Attributes.Add(landmarks_history_Established_year);


            Category landmarks_notes = new Category(1, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute landmarks_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute landmarks_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            landmarks_notes.Attributes.Add(landmarks_notes_Notes);
            landmarks_notes.Attributes.Add(landmarks_notes_Private_Notes);


            Category landmarks_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute landmarks_overview_Name = new Attribute("Name", "Name", "varchar", "Whats the name of this landmark");
            Attribute landmarks_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this landmark");
            Attribute landmarks_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this landmark known by");
            Attribute landmarks_overview_Type_of_landmark = new Attribute("Type_of_landmark", "Type Of Landmark", "varchar", "Write as little or as much as you want");
            Attribute landmarks_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this landmark.", "universes");
            Attribute landmarks_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your landmarks.");
            landmarks_overview.Attributes.Add(landmarks_overview_Name);
            landmarks_overview.Attributes.Add(landmarks_overview_Description);
            landmarks_overview.Attributes.Add(landmarks_overview_Other_Names);
            landmarks_overview.Attributes.Add(landmarks_overview_Type_of_landmark);
            landmarks_overview.Attributes.Add(landmarks_overview_Universe);
            landmarks_overview.Attributes.Add(landmarks_overview_Tags);


            Category landmarks_location = new Category(2, "Location", "<i class='material-icons' translate='no'>info</i>", "location");

            Attribute landmarks_location_Country = new Attribute("Country", "Country", "varchar", "");
            Attribute landmarks_location_Nearby_towns = new Attribute("Nearby_towns", "Nearby Towns", "varchar", "");
            landmarks_location.Attributes.Add(landmarks_location_Country);
            landmarks_location.Attributes.Add(landmarks_location_Nearby_towns);


            Category landmarks_appearance = new Category(3, "Eppearance", "<i class='material-icons' translate='no'>face</i>", "appearance");


            Category landmarks_gallery = new Category(1, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute landmarks_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            landmarks_gallery.Attributes.Add(landmarks_gallery_gallery);


            content_landmarks.categories.Add(landmarks_ecosystem);
            content_landmarks.categories.Add(landmarks_history);
            content_landmarks.categories.Add(landmarks_notes);
            content_landmarks.categories.Add(landmarks_overview);
            content_landmarks.categories.Add(landmarks_location);
            content_landmarks.categories.Add(landmarks_appearance);
            content_landmarks.references.Add(landmarks_gallery);


            content_landmarks.categories = content_landmarks.categories.OrderBy(c => c.Order).ToList();
            content_landmarks.categories.AddAutoIncrmentId("Id");

            #endregion
            #region languages
            Content content_languages = new Content("languages", true);

            Category languages_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute languages_overview_Name = new Attribute("Name", "Name", "varchar", "What is this language named");
            Attribute languages_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this language known by");
            Attribute languages_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this language.", "universes");
            Attribute languages_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your languages.");
            languages_overview.Attributes.Add(languages_overview_Name);
            languages_overview.Attributes.Add(languages_overview_Other_Names);
            languages_overview.Attributes.Add(languages_overview_Universe);
            languages_overview.Attributes.Add(languages_overview_Tags);


            Category languages_info = new Category(2, "Info", "<i class='material-icons' translate='no'>forum</i>", "info");

            Attribute languages_info_History = new Attribute("History", "History", "tinytext", "What is the history of this language");
            Attribute languages_info_Typology = new Attribute("Typology", "Typology", "varchar", "Write as little or as much as you want");
            Attribute languages_info_Dialectical_information = new Attribute("Dialectical_information", "Dialectical Information", "varchar", "Write as little or as much as you want");
            Attribute languages_info_Register = new Attribute("Register", "Register", "varchar", "Write as little or as much as you want");
            Attribute languages_info_Gestures = new Attribute("Gestures", "Gestures", "varchar", "What gestures are common for speakers of this language");
            Attribute languages_info_Evolution = new Attribute("Evolution", "Evolution", "varchar", "How has this language evolved over time");
            languages_info.Attributes.Add(languages_info_History);
            languages_info.Attributes.Add(languages_info_Typology);
            languages_info.Attributes.Add(languages_info_Dialectical_information);
            languages_info.Attributes.Add(languages_info_Register);
            languages_info.Attributes.Add(languages_info_Gestures);
            languages_info.Attributes.Add(languages_info_Evolution);


            Category languages_phonology = new Category(3, "Phonology", "<i class='material-icons' translate='no'>speaker_notes</i>", "phonology");

            Attribute languages_phonology_Phonology = new Attribute("Phonology", "Phonology", "varchar", "Write as little or as much as you want");
            languages_phonology.Attributes.Add(languages_phonology_Phonology);


            Category languages_grammar = new Category(4, "Grammar", "<i class='material-icons' translate='no'>list</i>", "grammar");

            Attribute languages_grammar_Grammar = new Attribute("Grammar", "Grammar", "varchar", "Write as little or as much as you want");
            languages_grammar.Attributes.Add(languages_grammar_Grammar);


            Category languages_vocabulary = new Category(5, "Vocabulary", "<i class='material-icons' translate='no'>book</i>", "vocabulary");

            Attribute languages_vocabulary_Please = new Attribute("Please", "Please", "varchar", "How do people speaking this language say please");
            Attribute languages_vocabulary_Trade = new Attribute("Trade", "Trade", "varchar", "What words or phrases are commonly used in when purchasingselling goods using this language");
            Attribute languages_vocabulary_Family = new Attribute("Family", "Family", "varchar", "What are the words for each family member in this language");
            Attribute languages_vocabulary_Body_parts = new Attribute("Body_parts", "Body Parts", "varchar", "What are the words for each body part in this language");
            Attribute languages_vocabulary_No_words = new Attribute("No_words", "No Words", "varchar", "How do people speaking this language say no");
            Attribute languages_vocabulary_Yes_words = new Attribute("Yes_words", "Yes Words", "varchar", "How do people speaking this language say yes");
            Attribute languages_vocabulary_Sorry = new Attribute("Sorry", "Sorry", "varchar", "How do people speaking this language say Im sorry");
            Attribute languages_vocabulary_You_are_welcome = new Attribute("You_are_welcome", "You Are Welcome", "varchar", "How do people speaking this language say you are welcome");
            Attribute languages_vocabulary_Thank_you = new Attribute("Thank_you", "Thank You", "varchar", "How do people speaking this language say thank you");
            Attribute languages_vocabulary_Goodbyes = new Attribute("Goodbyes", "Goodbyes", "varchar", "How do people speaking this language say goodbye to each other");
            Attribute languages_vocabulary_Greetings = new Attribute("Greetings", "Greetings", "varchar", "How do people speaking this language greet each other");
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


            Category languages_entities = new Category(6, "Entities", "<i class='material-icons' translate='no'>settings_input_component</i>", "entities");

            Attribute languages_entities_Numbers = new Attribute("Numbers", "Numbers", "varchar", "What are the words for each number in this language");
            Attribute languages_entities_Quantifiers = new Attribute("Quantifiers", "Quantifiers", "varchar", "What words are there for quantifiers a few many some etc in this language");
            Attribute languages_entities_Determiners = new Attribute("Determiners", "Determiners", "varchar", "What words are there for determiners the my this etc in this language");
            Attribute languages_entities_Pronouns = new Attribute("Pronouns", "Pronouns", "varchar", "What pronouns are there in this language");
            languages_entities.Attributes.Add(languages_entities_Numbers);
            languages_entities.Attributes.Add(languages_entities_Quantifiers);
            languages_entities.Attributes.Add(languages_entities_Determiners);
            languages_entities.Attributes.Add(languages_entities_Pronouns);


            Category languages_notes = new Category(8, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute languages_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute languages_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", "Write as little or as much as you want");
            languages_notes.Attributes.Add(languages_notes_Notes);
            languages_notes.Attributes.Add(languages_notes_Private_notes);

            Category languages_gallery = new Category(7, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute languages_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            languages_gallery.Attributes.Add(languages_gallery_gallery);


            content_languages.categories.Add(languages_overview);
            content_languages.categories.Add(languages_info);
            content_languages.categories.Add(languages_phonology);
            content_languages.categories.Add(languages_grammar);
            content_languages.categories.Add(languages_vocabulary);
            content_languages.categories.Add(languages_entities);
            content_languages.categories.Add(languages_notes);
            content_languages.references.Add(languages_gallery);


            content_languages.categories = content_languages.categories.OrderBy(c => c.Order).ToList();
            content_languages.categories.AddAutoIncrmentId("Id");

            #endregion
            #region locations
            Content content_locations = new Content("locations", true);

            Category locations_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute locations_overview_Name = new Attribute("Name", "Name", "varchar", "What is this locations full name");
            Attribute locations_overview_Type = new Attribute("Type", "Type", "varchar", "What type of location is this location");
            Attribute locations_overview_Description = new Attribute("Description", "Description", "text", "Describe this location.");
            Attribute locations_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this location.", "universes");
            Attribute locations_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your locations.");
            locations_overview.Attributes.Add(locations_overview_Name);
            locations_overview.Attributes.Add(locations_overview_Type);
            locations_overview.Attributes.Add(locations_overview_Description);
            locations_overview.Attributes.Add(locations_overview_Universe);
            locations_overview.Attributes.Add(locations_overview_Tags);


            Category locations_culture = new Category(2, "Culture", "<i class='material-icons' translate='no'>face</i>", "culture");

            Attribute locations_culture_Language = new Attribute("Language", "Language", "varchar", "What languages are spoken in this location");
            Attribute locations_culture_Population = new Attribute("Population", "Population", "double", "What is this locations population");
            Attribute locations_culture_Currency = new Attribute("Currency", "Currency", "varchar", "What currencies are used in this location");
            Attribute locations_culture_Motto = new Attribute("Motto", "Motto", "tinytext", "What is this locations motto");
            Attribute locations_culture_Laws = new Attribute("Laws", "Laws", "tinytext", "What are the laws in this location");
            Attribute locations_culture_Sports = new Attribute("Sports", "Sports", "varchar", "What sports are played in this location");
            Attribute locations_culture_Leaders = new Attribute("Leaders", "Leaders", "varchar", "This field allows you to link your other Notebook.ai pages to this location.");
            Attribute locations_culture_Spoken_Languages = new Attribute("Spoken_Languages", "Spoken Languages", "varchar", "This field allows you to link your other Notebook.ai pages to this location.");
            locations_culture.Attributes.Add(locations_culture_Language);
            locations_culture.Attributes.Add(locations_culture_Population);
            locations_culture.Attributes.Add(locations_culture_Currency);
            locations_culture.Attributes.Add(locations_culture_Motto);
            locations_culture.Attributes.Add(locations_culture_Laws);
            locations_culture.Attributes.Add(locations_culture_Sports);
            locations_culture.Attributes.Add(locations_culture_Leaders);
            locations_culture.Attributes.Add(locations_culture_Spoken_Languages);


            Category locations_cities = new Category(3, "Cities", "<i class='material-icons' translate='no'>business</i>", "cities");

            Attribute locations_cities_Capital_cities = new Attribute("Capital_cities", "Capital Cities", "varchar", "This field allows you to link your other Notebook.ai pages to this location.");
            Attribute locations_cities_Largest_cities = new Attribute("Largest_cities", "Largest Cities", "varchar", "This field allows you to link your other Notebook.ai pages to this location.");
            Attribute locations_cities_Notable_cities = new Attribute("Notable_cities", "Notable Cities", "varchar", "This field allows you to link your other Notebook.ai pages to this location.");
            locations_cities.Attributes.Add(locations_cities_Capital_cities);
            locations_cities.Attributes.Add(locations_cities_Largest_cities);
            locations_cities.Attributes.Add(locations_cities_Notable_cities);


            Category locations_geography = new Category(4, "Geography", "<i class='material-icons' translate='no'>map</i>", "geography");

            Attribute locations_geography_Area = new Attribute("Area", "Area", "double", "What kind of area is this location in");
            Attribute locations_geography_Crops = new Attribute("Crops", "Crops", "varchar", "What crops does this location produce");
            Attribute locations_geography_Located_at = new Attribute("Located_at", "Located At", "varchar", "Where is this location located");
            Attribute locations_geography_Climate = new Attribute("Climate", "Climate", "tinytext", "What is the climate like in this location");
            Attribute locations_geography_Landmarks = new Attribute("Landmarks", "Landmarks", "varchar", "This field allows you to link your other Notebook.ai pages to this location.");
            locations_geography.Attributes.Add(locations_geography_Area);
            locations_geography.Attributes.Add(locations_geography_Crops);
            locations_geography.Attributes.Add(locations_geography_Located_at);
            locations_geography.Attributes.Add(locations_geography_Climate);
            locations_geography.Attributes.Add(locations_geography_Landmarks);


            Category locations_history = new Category(5, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute locations_history_Notable_Wars = new Attribute("Notable_Wars", "Notable Wars", "tinytext", "What notable wars has this location been involved in");
            Attribute locations_history_Founding_Story = new Attribute("Founding_Story", "Founding Story", "tinytext", "How was this location founded");
            Attribute locations_history_Established_Year = new Attribute("Established_Year", "Established Year", "int", "When was this location established");
            locations_history.Attributes.Add(locations_history_Notable_Wars);
            locations_history.Attributes.Add(locations_history_Founding_Story);
            locations_history.Attributes.Add(locations_history_Established_Year);


            Category locations_notes = new Category(7, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute locations_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute locations_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            locations_notes.Attributes.Add(locations_notes_Notes);
            locations_notes.Attributes.Add(locations_notes_Private_Notes);

            Category locations_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute locations_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            locations_gallery.Attributes.Add(locations_gallery_gallery);


            content_locations.categories.Add(locations_overview);
            content_locations.categories.Add(locations_culture);
            content_locations.categories.Add(locations_cities);
            content_locations.categories.Add(locations_geography);
            content_locations.categories.Add(locations_history);
            content_locations.categories.Add(locations_notes);
            content_locations.references.Add(locations_gallery);


            content_locations.categories = content_locations.categories.OrderBy(c => c.Order).ToList();
            content_locations.categories.AddAutoIncrmentId("Id");

            #endregion
            #region lores
            Content content_lores = new Content("lores", true);

            Category lores_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute lores_overview_Name = new Attribute("Name", "Name", "varchar", "What is this lores name");
            Attribute lores_overview_Summary = new Attribute("Summary", "Summary", "text", "In short what happens in this lore");
            Attribute lores_overview_Type = new Attribute("Type", "Type", "varchar", "What type of lore is this");
            Attribute lores_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this lore.", "universes");
            Attribute lores_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your lores.");
            lores_overview.Attributes.Add(lores_overview_Name);
            lores_overview.Attributes.Add(lores_overview_Summary);
            lores_overview.Attributes.Add(lores_overview_Type);
            lores_overview.Attributes.Add(lores_overview_Universe);
            lores_overview.Attributes.Add(lores_overview_Tags);


            Category lores_content = new Category(2, "Content", "<i class='material-icons' translate='no'>import_contacts</i>", "content");

            Attribute lores_content_Full_text = new Attribute("Full_text", "Full Text", "text", "What is the full text of this lore");
            Attribute lores_content_Dialect = new Attribute("Dialect", "Dialect", "varchar", "What dialects are present in this lore");
            Attribute lores_content_Structure = new Attribute("Structure", "Structure", "varchar", "What form or structure does the written form of this lore take");
            Attribute lores_content_Tone = new Attribute("Tone", "Tone", "varchar", "What is the overall tone of this lore");
            Attribute lores_content_Genre = new Attribute("Genre", "Genre", "varchar", "What is the genre of this lore");
            lores_content.Attributes.Add(lores_content_Full_text);
            lores_content.Attributes.Add(lores_content_Dialect);
            lores_content.Attributes.Add(lores_content_Structure);
            lores_content.Attributes.Add(lores_content_Tone);
            lores_content.Attributes.Add(lores_content_Genre);


            Category lores_setting = new Category(3, "Setting", "<i class='material-icons' translate='no'>home</i>", "setting");

            Attribute lores_setting_Time_period = new Attribute("Time_period", "Time Period", "varchar", "What time periods does this lore take place in");
            Attribute lores_setting_Planets = new Attribute("Planets", "Planets", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_setting_Continents = new Attribute("Continents", "Continents", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_setting_Countries = new Attribute("Countries", "Countries", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_setting_Landmarks = new Attribute("Landmarks", "Landmarks", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_setting_Towns = new Attribute("Towns", "Towns", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_setting_Buildings = new Attribute("Buildings", "Buildings", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_setting_Schools = new Attribute("Schools", "Schools", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            lores_setting.Attributes.Add(lores_setting_Time_period);
            lores_setting.Attributes.Add(lores_setting_Planets);
            lores_setting.Attributes.Add(lores_setting_Continents);
            lores_setting.Attributes.Add(lores_setting_Countries);
            lores_setting.Attributes.Add(lores_setting_Landmarks);
            lores_setting.Attributes.Add(lores_setting_Towns);
            lores_setting.Attributes.Add(lores_setting_Buildings);
            lores_setting.Attributes.Add(lores_setting_Schools);


            Category lores_about = new Category(4, "About", "<i class='material-icons' translate='no'>find_in_page</i>", "about");

            Attribute lores_about_Subjects = new Attribute("Subjects", "Subjects", "varchar", "What subjects does this lore cover");
            Attribute lores_about_Characters = new Attribute("Characters", "Characters", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Deities = new Attribute("Deities", "Deities", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Creatures = new Attribute("Creatures", "Creatures", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Floras = new Attribute("Floras", "Floras", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Jobs = new Attribute("Jobs", "Jobs", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Technologies = new Attribute("Technologies", "Technologies", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Vehicles = new Attribute("Vehicles", "Vehicles", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Conditions = new Attribute("Conditions", "Conditions", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Races = new Attribute("Races", "Races", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Religions = new Attribute("Religions", "Religions", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Magic = new Attribute("Magic", "Magic", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Governments = new Attribute("Governments", "Governments", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Groups = new Attribute("Groups", "Groups", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Traditions = new Attribute("Traditions", "Traditions", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Foods = new Attribute("Foods", "Foods", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_about_Sports = new Attribute("Sports", "Sports", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
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


            Category lores_truthiness = new Category(5, "Truthiness", "<i class='material-icons' translate='no'>thumbs_up_down</i>", "truthiness");

            Attribute lores_truthiness_True_parts = new Attribute("True_parts", "True Parts", "varchar", "What parts of this lore are based on truth");
            Attribute lores_truthiness_false_parts = new Attribute("false_parts", "false Parts", "varchar", "What parts of this lore arent based on truth");
            Attribute lores_truthiness_Believability = new Attribute("Believability", "Believability", "varchar", "What makes this lore believable to those who hear it");
            Attribute lores_truthiness_Morals = new Attribute("Morals", "Morals", "varchar", "What morals does this lore touch on What is the primary moral of this lore");
            Attribute lores_truthiness_Symbolisms = new Attribute("Symbolisms", "Symbolisms", "varchar", "What symbolism is there in this lore");
            Attribute lores_truthiness_Believers = new Attribute("Believers", "Believers", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_truthiness_Hoaxes = new Attribute("Hoaxes", "Hoaxes", "varchar", "What hoaxes have popped up from this lore");
            lores_truthiness.Attributes.Add(lores_truthiness_True_parts);
            lores_truthiness.Attributes.Add(lores_truthiness_false_parts);
            lores_truthiness.Attributes.Add(lores_truthiness_Believability);
            lores_truthiness.Attributes.Add(lores_truthiness_Morals);
            lores_truthiness.Attributes.Add(lores_truthiness_Symbolisms);
            lores_truthiness.Attributes.Add(lores_truthiness_Believers);
            lores_truthiness.Attributes.Add(lores_truthiness_Hoaxes);


            Category lores_culture = new Category(6, "Culture", "<i class='material-icons' translate='no'>face</i>", "culture");

            Attribute lores_culture_Impact = new Attribute("Impact", "Impact", "tinytext", "What impact has this lore had on culture over time");
            Attribute lores_culture_Created_traditions = new Attribute("Created_traditions", "Created Traditions", "tinytext", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_culture_Influence_on_modern_times = new Attribute("Influence_on_modern_times", "Influence On Modern Times", "tinytext", "Write as little or as much as you want");
            Attribute lores_culture_Motivations = new Attribute("Motivations", "Motivations", "varchar", "Why do people tell this lore Why was this lore first written");
            Attribute lores_culture_Reception = new Attribute("Reception", "Reception", "tinytext", "How has this lore been received by the public");
            Attribute lores_culture_Interpretations = new Attribute("Interpretations", "Interpretations", "tinytext", "What different interpretations have existed for this lore");
            Attribute lores_culture_Media_adaptations = new Attribute("Media_adaptations", "Media Adaptations", "tinytext", "How has this lore been adapted into popular media");
            Attribute lores_culture_Criticism = new Attribute("Criticism", "Criticism", "tinytext", "What criticism is there against this lore");
            Attribute lores_culture_Created_phrases = new Attribute("Created_phrases", "Created Phrases", "tinytext", "What phrases or slang originated from this lore");
            lores_culture.Attributes.Add(lores_culture_Impact);
            lores_culture.Attributes.Add(lores_culture_Created_traditions);
            lores_culture.Attributes.Add(lores_culture_Influence_on_modern_times);
            lores_culture.Attributes.Add(lores_culture_Motivations);
            lores_culture.Attributes.Add(lores_culture_Reception);
            lores_culture.Attributes.Add(lores_culture_Interpretations);
            lores_culture.Attributes.Add(lores_culture_Media_adaptations);
            lores_culture.Attributes.Add(lores_culture_Criticism);
            lores_culture.Attributes.Add(lores_culture_Created_phrases);


            Category lores_origin = new Category(7, "Origin", "<i class='material-icons' translate='no'>today</i>", "origin");

            Attribute lores_origin_Source = new Attribute("Source", "Source", "tinytext", "Where and how did this lore originate");
            Attribute lores_origin_Original_telling = new Attribute("Original_telling", "Original Telling", "tinytext", "What was the first telling of this lore like");
            Attribute lores_origin_Date_recorded = new Attribute("Date_recorded", "Date Recorded", "varchar", "When was the first recording of this lore");
            Attribute lores_origin_Inspirations = new Attribute("Inspirations", "Inspirations", "tinytext", "What inspirations were there when coming up with this lore");
            Attribute lores_origin_Original_author = new Attribute("Original_author", "Original Author", "varchar", "Who were the original authors of this lore");
            Attribute lores_origin_Original_languages = new Attribute("Original_languages", "Original Languages", "varchar", "This field allows you to link your other Notebook.ai pages to this lore.");
            lores_origin.Attributes.Add(lores_origin_Source);
            lores_origin.Attributes.Add(lores_origin_Original_telling);
            lores_origin.Attributes.Add(lores_origin_Date_recorded);
            lores_origin.Attributes.Add(lores_origin_Inspirations);
            lores_origin.Attributes.Add(lores_origin_Original_author);
            lores_origin.Attributes.Add(lores_origin_Original_languages);


            Category lores_history = new Category(8, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute lores_history_Historical_context = new Attribute("Historical_context", "Historical Context", "text", "What historical information is relevant to better understand this lore");
            Attribute lores_history_Background_information = new Attribute("Background_information", "Background Information", "tinytext", "What background information is relevant to better understand this lore");
            Attribute lores_history_Important_translations = new Attribute("Important_translations", "Important Translations", "tinytext", "Which translations of this lore were the most important");
            Attribute lores_history_Propagation_method = new Attribute("Propagation_method", "Propagation Method", "tinytext", "How is this lore retold How does the story of this lore spread");
            lores_history.Attributes.Add(lores_history_Historical_context);
            lores_history.Attributes.Add(lores_history_Background_information);
            lores_history.Attributes.Add(lores_history_Important_translations);
            lores_history.Attributes.Add(lores_history_Propagation_method);


            Category lores_variations = new Category(9, "Variations", "<i class='material-icons' translate='no'>file_copy</i>", "variations");

            Attribute lores_variations_Geographical_variations = new Attribute("Geographical_variations", "Geographical Variations", "text", "What geographical areas have different tellings of this lore");
            Attribute lores_variations_Evolution_over_time = new Attribute("Evolution_over_time", "Evolution Over Time", "tinytext", "Write as little or as much as you want");
            Attribute lores_variations_Translation_variations = new Attribute("Translation_variations", "Translation Variations", "tinytext", "Which translations of this lore have significantly changed the meaning");
            Attribute lores_variations_Variations = new Attribute("Variations", "Variations", "tinytext", "This field allows you to link your other Notebook.ai pages to this lore.");
            Attribute lores_variations_Related_lores = new Attribute("Related_lores", "Related Lores", "tinytext", "This field allows you to link your other Notebook.ai pages to this lore.");
            lores_variations.Attributes.Add(lores_variations_Geographical_variations);
            lores_variations.Attributes.Add(lores_variations_Evolution_over_time);
            lores_variations.Attributes.Add(lores_variations_Translation_variations);
            lores_variations.Attributes.Add(lores_variations_Variations);
            lores_variations.Attributes.Add(lores_variations_Related_lores);


            Category lores_notes = new Category(11, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute lores_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute lores_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            lores_notes.Attributes.Add(lores_notes_Notes);
            lores_notes.Attributes.Add(lores_notes_Private_Notes);

            Category lores_gallery = new Category(10, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute lores_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            lores_gallery.Attributes.Add(lores_gallery_gallery);


            content_lores.categories.Add(lores_overview);
            content_lores.categories.Add(lores_content);
            content_lores.categories.Add(lores_setting);
            content_lores.categories.Add(lores_about);
            content_lores.categories.Add(lores_truthiness);
            content_lores.categories.Add(lores_culture);
            content_lores.categories.Add(lores_origin);
            content_lores.categories.Add(lores_history);
            content_lores.categories.Add(lores_variations);
            content_lores.categories.Add(lores_notes);
            content_lores.references.Add(lores_gallery);


            content_lores.categories = content_lores.categories.OrderBy(c => c.Order).ToList();
            content_lores.categories.AddAutoIncrmentId("Id");

            #endregion
            #region magics
            Content content_magics = new Content("magics", true);

            Category magics_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute magics_overview_Name = new Attribute("Name", "Name", "varchar", "What is the name of this magic");
            Attribute magics_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this magic");
            Attribute magics_overview_Type_of_magic = new Attribute("Type_of_magic", "Type Of Magic", "tinytext", "What type of magic is this magic");
            Attribute magics_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this magic.", "universes");
            Attribute magics_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your magics.");
            magics_overview.Attributes.Add(magics_overview_Name);
            magics_overview.Attributes.Add(magics_overview_Description);
            magics_overview.Attributes.Add(magics_overview_Type_of_magic);
            magics_overview.Attributes.Add(magics_overview_Universe);
            magics_overview.Attributes.Add(magics_overview_Tags);


            Category magics_appearance = new Category(2, "Appearance", "<i class='material-icons' translate='no'>flash_on</i>", "appearance");

            Attribute magics_appearance_Effects = new Attribute("Effects", "Effects", "tinytext", "What effects does this magic have");
            Attribute magics_appearance_Visuals = new Attribute("Visuals", "Visuals", "tinytext", "What do the visual effects of this magic look like");
            Attribute magics_appearance_Aftereffects = new Attribute("Aftereffects", "Aftereffects", "tinytext", "What visual effects persist after this magic");
            magics_appearance.Attributes.Add(magics_appearance_Effects);
            magics_appearance.Attributes.Add(magics_appearance_Visuals);
            magics_appearance.Attributes.Add(magics_appearance_Aftereffects);


            Category magics_effects = new Category(3, "Effects", "<i class='material-icons' translate='no'>flare</i>", "effects");

            Attribute magics_effects_Positive_effects = new Attribute("Positive_effects", "Positive Effects", "tinytext", "What positive effects does this magic have");
            Attribute magics_effects_Neutral_effects = new Attribute("Neutral_effects", "Neutral Effects", "tinytext", "What neutral effects does this magic have");
            Attribute magics_effects_Negative_effects = new Attribute("Negative_effects", "Negative Effects", "tinytext", "What negative effects does this magic have");
            Attribute magics_effects_Conditions = new Attribute("Conditions", "Conditions", "tinytext", "What conditions can this magic afflict");
            Attribute magics_effects_Scale = new Attribute("Scale", "Scale", "double", "How does the power level of this magic scale");
            magics_effects.Attributes.Add(magics_effects_Positive_effects);
            magics_effects.Attributes.Add(magics_effects_Neutral_effects);
            magics_effects.Attributes.Add(magics_effects_Negative_effects);
            magics_effects.Attributes.Add(magics_effects_Conditions);
            magics_effects.Attributes.Add(magics_effects_Scale);


            Category magics_alignment = new Category(4, "Alignment", "<i class='material-icons' translate='no'>polymer</i>", "alignment");

            Attribute magics_alignment_Element = new Attribute("Element", "Element", "tinytext", "What element is this magic most closely aligned to");
            Attribute magics_alignment_Deities = new Attribute("Deities", "Deities", "tinytext", "This field allows you to link your other Notebook.ai pages to this magic.");
            magics_alignment.Attributes.Add(magics_alignment_Element);
            magics_alignment.Attributes.Add(magics_alignment_Deities);


            Category magics_requirements = new Category(5, "Requirements", "<i class='material-icons' translate='no'>lock</i>", "requirements");

            Attribute magics_requirements_Resource_costs = new Attribute("Resource_costs", "Resource Costs", "tinytext", "What resource costs are required to use this magic");
            Attribute magics_requirements_Materials_required = new Attribute("Materials_required", "Materials Required", "tinytext", "What materials are required to use this magic");
            Attribute magics_requirements_Skills_required = new Attribute("Skills_required", "Skills Required", "tinytext", "What skills are required to use this magic");
            Attribute magics_requirements_Limitations = new Attribute("Limitations", "Limitations", "tinytext", "What limitations does this magic have What cant it do");
            Attribute magics_requirements_Education = new Attribute("Education", "Education", "tinytext", "What needs to be learned before this magic can be used");
            magics_requirements.Attributes.Add(magics_requirements_Resource_costs);
            magics_requirements.Attributes.Add(magics_requirements_Materials_required);
            magics_requirements.Attributes.Add(magics_requirements_Skills_required);
            magics_requirements.Attributes.Add(magics_requirements_Limitations);
            magics_requirements.Attributes.Add(magics_requirements_Education);


            Category magics_notes = new Category(7, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute magics_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute magics_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", "Write as little or as much as you want");
            magics_notes.Attributes.Add(magics_notes_Notes);
            magics_notes.Attributes.Add(magics_notes_Private_notes);

            Category magics_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute magics_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            magics_gallery.Attributes.Add(magics_gallery_gallery);


            content_magics.categories.Add(magics_overview);
            content_magics.categories.Add(magics_appearance);
            content_magics.categories.Add(magics_effects);
            content_magics.categories.Add(magics_alignment);
            content_magics.categories.Add(magics_requirements);
            content_magics.categories.Add(magics_notes);
            content_magics.references.Add(magics_gallery);


            content_magics.categories = content_magics.categories.OrderBy(c => c.Order).ToList();
            content_magics.categories.AddAutoIncrmentId("Id");

            #endregion
            #region planets
            Content content_planets = new Content("planets", true);

            Category planets_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute planets_overview_Name = new Attribute("Name", "Name", "varchar", "Whats the name of this planet");
            Attribute planets_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this planet");
            Attribute planets_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this planet.", "universes");
            Attribute planets_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your planets.");
            planets_overview.Attributes.Add(planets_overview_Name);
            planets_overview.Attributes.Add(planets_overview_Description);
            planets_overview.Attributes.Add(planets_overview_Universe);
            planets_overview.Attributes.Add(planets_overview_Tags);


            Category planets_geography = new Category(2, "Geography", "<i class='material-icons' translate='no'>layers</i>", "geography");

            Attribute planets_geography_Size = new Attribute("Size", "Size", "double", "How big is this planet");
            Attribute planets_geography_Surface = new Attribute("Surface", "Surface", "double", "What is the surface of this planet like");
            Attribute planets_geography_Climate = new Attribute("Climate", "Climate", "tinytext", "Whats the climate on this planet like");
            Attribute planets_geography_Calendar_System = new Attribute("Calendar_System", "Calendar System", "tinytext", "");
            Attribute planets_geography_Weather = new Attribute("Weather", "Weather", "tinytext", "Whats the weather like on this planet");
            Attribute planets_geography_Water_Content = new Attribute("Water_Content", "Water Content", "double", "Whats the water content like on this planet");
            Attribute planets_geography_Natural_Resources = new Attribute("Natural_Resources", "Natural Resources", "tinytext", "What natural resources are there on this planet");
            Attribute planets_geography_Continents = new Attribute("Continents", "Continents", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_geography_Countries = new Attribute("Countries", "Countries", "tinytext", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_geography_Locations = new Attribute("Locations", "Locations", "tinytext", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_geography_Landmarks = new Attribute("Landmarks", "Landmarks", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
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


            Category planets_climate = new Category(3, "Climate", "<i class='material-icons' translate='no'>fireplace</i>", "climate");

            Attribute planets_climate_Seasons = new Attribute("Seasons", "Seasons", "tinytext", "What seasons are there on this planet");
            Attribute planets_climate_Temperature = new Attribute("Temperature", "Temperature", "double", "How hot does this planet get How cold Whats the average temperature");
            Attribute planets_climate_Atmosphere = new Attribute("Atmosphere", "Atmosphere", "tinytext", "What does the atmosphere consist of on this planet");
            Attribute planets_climate_Natural_diasters = new Attribute("Natural_diasters", "Natural Diasters", "tinytext", "");
            planets_climate.Attributes.Add(planets_climate_Seasons);
            planets_climate.Attributes.Add(planets_climate_Temperature);
            planets_climate.Attributes.Add(planets_climate_Atmosphere);
            planets_climate.Attributes.Add(planets_climate_Natural_diasters);


            Category planets_time = new Category(4, "Time", "<i class='material-icons' translate='no'>hourglass_empty</i>", "time");

            Attribute planets_time_Length_Of_Day = new Attribute("Length_Of_Day", "Length Of Day", "double", "How long is daytime on this planet");
            Attribute planets_time_Length_Of_Night = new Attribute("Length_Of_Night", "Length Of Night", "double", "How long is night time on this planet");
            Attribute planets_time_Night_sky = new Attribute("Night_sky", "Night Sky", "tinytext", "What does the sky look like at night on this planet");
            Attribute planets_time_Day_sky = new Attribute("Day_sky", "Day Sky", "tinytext", "What does the sky look like during the day on this planet");
            planets_time.Attributes.Add(planets_time_Length_Of_Day);
            planets_time.Attributes.Add(planets_time_Length_Of_Night);
            planets_time.Attributes.Add(planets_time_Night_sky);
            planets_time.Attributes.Add(planets_time_Day_sky);


            Category planets_inhabitants = new Category(5, "Inhabitants", "<i class='material-icons' translate='no'>face</i>", "inhabitants");

            Attribute planets_inhabitants_Population = new Attribute("Population", "Population", "double", "Whats the population like on this planet");
            Attribute planets_inhabitants_Races = new Attribute("Races", "Races", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_inhabitants_Flora = new Attribute("Flora", "Flora", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_inhabitants_Creatures = new Attribute("Creatures", "Creatures", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_inhabitants_Religions = new Attribute("Religions", "Religions", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_inhabitants_Deities = new Attribute("Deities", "Deities", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_inhabitants_Groups = new Attribute("Groups", "Groups", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_inhabitants_Languages = new Attribute("Languages", "Languages", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            Attribute planets_inhabitants_Towns = new Attribute("Towns", "Towns", "tinytext", "This field allows you to link your other Notebook.ai pages to this planet.");
            planets_inhabitants.Attributes.Add(planets_inhabitants_Population);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Races);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Flora);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Creatures);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Religions);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Deities);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Groups);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Languages);
            planets_inhabitants.Attributes.Add(planets_inhabitants_Towns);


            Category planets_astral = new Category(6, "Astral", "<i class='material-icons' translate='no'>star_border</i>", "astral");

            Attribute planets_astral_Suns = new Attribute("Suns", "Suns", "varchar", "Does this planet have any suns");
            Attribute planets_astral_Moons = new Attribute("Moons", "Moons", "varchar", "Does this planet have any moons If so what are they");
            Attribute planets_astral_Orbit = new Attribute("Orbit", "Orbit", "tinytext", "What does this planets astral orbit look like");
            Attribute planets_astral_Visible_Constellations = new Attribute("Visible_Constellations", "Visible Constellations", "tinytext", "What constellations are visible from this planet");
            Attribute planets_astral_Nearby_planets = new Attribute("Nearby_planets", "Nearby Planets", "varchar", "This field allows you to link your other Notebook.ai pages to this planet.");
            planets_astral.Attributes.Add(planets_astral_Suns);
            planets_astral.Attributes.Add(planets_astral_Moons);
            planets_astral.Attributes.Add(planets_astral_Orbit);
            planets_astral.Attributes.Add(planets_astral_Visible_Constellations);
            planets_astral.Attributes.Add(planets_astral_Nearby_planets);


            Category planets_history = new Category(7, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute planets_history_First_Inhabitants_Story = new Attribute("First_Inhabitants_Story", "First Inhabitants Story", "text", "How did the first inhabitants arrive on this planet");
            Attribute planets_history_World_History = new Attribute("World_History", "World History", "text", "What is the world history of this planet");
            planets_history.Attributes.Add(planets_history_First_Inhabitants_Story);
            planets_history.Attributes.Add(planets_history_World_History);


            Category planets_notes = new Category(9, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute planets_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute planets_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            planets_notes.Attributes.Add(planets_notes_Notes);
            planets_notes.Attributes.Add(planets_notes_Private_Notes);

            Category planets_gallery = new Category(8, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute planets_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            planets_gallery.Attributes.Add(planets_gallery_gallery);


            content_planets.categories.Add(planets_overview);
            content_planets.categories.Add(planets_geography);
            content_planets.categories.Add(planets_climate);
            content_planets.categories.Add(planets_time);
            content_planets.categories.Add(planets_inhabitants);
            content_planets.categories.Add(planets_astral);
            content_planets.categories.Add(planets_history);
            content_planets.categories.Add(planets_notes);
            content_planets.references.Add(planets_gallery);


            content_planets.categories = content_planets.categories.OrderBy(c => c.Order).ToList();
            content_planets.categories.AddAutoIncrmentId("Id");

            #endregion
            #region races
            Content content_races = new Content("races", true);

            Category races_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute races_overview_Name = new Attribute("Name", "Name", "varchar", "What is this race named");
            Attribute races_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this races people");
            Attribute races_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names do this race have");
            Attribute races_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this race.", "universes");
            Attribute races_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your races.");
            races_overview.Attributes.Add(races_overview_Name);
            races_overview.Attributes.Add(races_overview_Description);
            races_overview.Attributes.Add(races_overview_Other_Names);
            races_overview.Attributes.Add(races_overview_Universe);
            races_overview.Attributes.Add(races_overview_Tags);


            Category races_looks = new Category(2, "Looks", "<i class='material-icons' translate='no'>face</i>", "looks");

            Attribute races_looks_Body_shape = new Attribute("Body_shape", "Body Shape", "double", "What does the average this race body shape look like");
            Attribute races_looks_Skin_colors = new Attribute("Skin_colors", "Skin Colors", "varchar", "What skin colors are common on the this race race");
            Attribute races_looks_General_height = new Attribute("General_height", "General Height", "double", "How tall is the average this race");
            Attribute races_looks_General_weight = new Attribute("General_weight", "General Weight", "double", "How heavy is the average this race");
            Attribute races_looks_Notable_features = new Attribute("Notable_features", "Notable Features", "tinytext", "What physical features on a this race are most noticeable");
            Attribute races_looks_Physical_variance = new Attribute("Physical_variance", "Physical Variance", "tinytext", "How much variance is there between individuals of the this race race");
            Attribute races_looks_Typical_clothing = new Attribute("Typical_clothing", "Typical Clothing", "tinytext", "What kind of clothing is common with this race individuals");
            races_looks.Attributes.Add(races_looks_Body_shape);
            races_looks.Attributes.Add(races_looks_Skin_colors);
            races_looks.Attributes.Add(races_looks_General_height);
            races_looks.Attributes.Add(races_looks_General_weight);
            races_looks.Attributes.Add(races_looks_Notable_features);
            races_looks.Attributes.Add(races_looks_Physical_variance);
            races_looks.Attributes.Add(races_looks_Typical_clothing);


            Category races_traits = new Category(3, "Traits", "<i class='material-icons' translate='no'>fingerprint</i>", "traits");

            Attribute races_traits_Strengths = new Attribute("Strengths", "Strengths", "tinytext", "What are the strengths of this race");
            Attribute races_traits_Weaknesses = new Attribute("Weaknesses", "Weaknesses", "tinytext", "What are the weaknesses of this race");
            Attribute races_traits_Conditions = new Attribute("Conditions", "Conditions", "tinytext", "What conditions are common with this race");
            races_traits.Attributes.Add(races_traits_Strengths);
            races_traits.Attributes.Add(races_traits_Weaknesses);
            races_traits.Attributes.Add(races_traits_Conditions);


            Category races_culture = new Category(4, "Culture", "<i class='material-icons' translate='no'>groups</i>", "culture");

            Attribute races_culture_Famous_figures = new Attribute("Famous_figures", "Famous Figures", "tinytext", "This field allows you to link your other Notebook.ai pages to this race.");
            Attribute races_culture_Traditions = new Attribute("Traditions", "Traditions", "tinytext", "What traditions are common with this races individuals");
            Attribute races_culture_Beliefs = new Attribute("Beliefs", "Beliefs", "tinytext", "What beliefs are commonly held by the this race");
            Attribute races_culture_Governments = new Attribute("Governments", "Governments", "varchar", "What governments are common with the this race");
            Attribute races_culture_Technologies = new Attribute("Technologies", "Technologies", "varchar", "What kinds of technologies do the this race societies take advantage of");
            Attribute races_culture_Occupations = new Attribute("Occupations", "Occupations", "tinytext", "What occupations are common with this race individuals");
            Attribute races_culture_Economics = new Attribute("Economics", "Economics", "tinytext", "What does the economic situation look like for the this race");
            Attribute races_culture_Favorite_foods = new Attribute("Favorite_foods", "Favorite Foods", "tinytext", "What are the most common favorite foods of the this race");
            races_culture.Attributes.Add(races_culture_Famous_figures);
            races_culture.Attributes.Add(races_culture_Traditions);
            races_culture.Attributes.Add(races_culture_Beliefs);
            races_culture.Attributes.Add(races_culture_Governments);
            races_culture.Attributes.Add(races_culture_Technologies);
            races_culture.Attributes.Add(races_culture_Occupations);
            races_culture.Attributes.Add(races_culture_Economics);
            races_culture.Attributes.Add(races_culture_Favorite_foods);


            Category races_history = new Category(5, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute races_history_Notable_events = new Attribute("Notable_events", "Notable Events", "text", "What events are most important to the history of this race");
            races_history.Attributes.Add(races_history_Notable_events);


            Category races_notes = new Category(7, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute races_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute races_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", "Write as little or as much as you want");
            races_notes.Attributes.Add(races_notes_Notes);
            races_notes.Attributes.Add(races_notes_Private_notes);

            Category races_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute races_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            races_gallery.Attributes.Add(races_gallery_gallery);


            content_races.categories.Add(races_overview);
            content_races.categories.Add(races_looks);
            content_races.categories.Add(races_traits);
            content_races.categories.Add(races_culture);
            content_races.categories.Add(races_history);
            content_races.categories.Add(races_notes);
            content_races.references.Add(races_gallery);


            content_races.categories = content_races.categories.OrderBy(c => c.Order).ToList();
            content_races.categories.AddAutoIncrmentId("Id");

            #endregion
            #region religions
            Content content_religions = new Content("religions", true);

            Category religions_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute religions_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your religions.");
            Attribute religions_overview_Name = new Attribute("Name", "Name", "varchar", "What is this religions name");
            Attribute religions_overview_Description = new Attribute("Description", "Description", "text", "How is this religion usually described");
            Attribute religions_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this religion known by");
            Attribute religions_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this religion.", "universes");
            religions_overview.Attributes.Add(religions_overview_Tags);
            religions_overview.Attributes.Add(religions_overview_Name);
            religions_overview.Attributes.Add(religions_overview_Description);
            religions_overview.Attributes.Add(religions_overview_Other_Names);
            religions_overview.Attributes.Add(religions_overview_Universe);


            Category religions_history = new Category(2, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute religions_history_Origin_story = new Attribute("Origin_story", "Origin Story", "text", "How did this religion first come into existence");
            Attribute religions_history_Notable_figures = new Attribute("Notable_figures", "Notable Figures", "tinytext", "This field allows you to link your other Notebook.ai pages to this religion.");
            Attribute religions_history_Artifacts = new Attribute("Artifacts", "Artifacts", "tinytext", "This field allows you to link your other Notebook.ai pages to this religion.");
            religions_history.Attributes.Add(religions_history_Origin_story);
            religions_history.Attributes.Add(religions_history_Notable_figures);
            religions_history.Attributes.Add(religions_history_Artifacts);


            Category religions_beliefs = new Category(3, "Beliefs", "<i class='material-icons' translate='no'>forum</i>", "beliefs");

            Attribute religions_beliefs_Deities = new Attribute("Deities", "Deities", "tinytext", "This field allows you to link your other Notebook.ai pages to this religion.");
            Attribute religions_beliefs_Teachings = new Attribute("Teachings", "Teachings", "tinytext", "What values does this religion teach");
            Attribute religions_beliefs_Prophecies = new Attribute("Prophecies", "Prophecies", "tinytext", "What prophecies does this religion teach");
            Attribute religions_beliefs_Places_of_worship = new Attribute("Places_of_worship", "Places Of Worship", "tinytext", "Where does this religion worship happen");
            Attribute religions_beliefs_Vision_of_paradise = new Attribute("Vision_of_paradise", "Vision Of Paradise", "tinytext", "What does the this religion vision of paradise look like");
            Attribute religions_beliefs_Obligations = new Attribute("Obligations", "Obligations", "tinytext", "What kinds of obligations are required of those who practice this religion");
            Attribute religions_beliefs_Worship_services = new Attribute("Worship_services", "Worship Services", "tinytext", "What kind of worship services are common with this religion");
            religions_beliefs.Attributes.Add(religions_beliefs_Deities);
            religions_beliefs.Attributes.Add(religions_beliefs_Teachings);
            religions_beliefs.Attributes.Add(religions_beliefs_Prophecies);
            religions_beliefs.Attributes.Add(religions_beliefs_Places_of_worship);
            religions_beliefs.Attributes.Add(religions_beliefs_Vision_of_paradise);
            religions_beliefs.Attributes.Add(religions_beliefs_Obligations);
            religions_beliefs.Attributes.Add(religions_beliefs_Worship_services);


            Category religions_traditions = new Category(4, "Traditions", "<i class='material-icons' translate='no'>account_balance</i>", "traditions");

            Attribute religions_traditions_Initiation_process = new Attribute("Initiation_process", "Initiation Process", "tinytext", "What does the this religion initiation process entail");
            Attribute religions_traditions_Rituals = new Attribute("Rituals", "Rituals", "tinytext", "What rituals are common with this religion");
            Attribute religions_traditions_Holidays = new Attribute("Holidays", "Holidays", "tinytext", "What are the this religion holidays");
            Attribute religions_traditions_Traditions = new Attribute("Traditions", "Traditions", "varchar", "What traditions are common with this religion");
            religions_traditions.Attributes.Add(religions_traditions_Initiation_process);
            religions_traditions.Attributes.Add(religions_traditions_Rituals);
            religions_traditions.Attributes.Add(religions_traditions_Holidays);
            religions_traditions.Attributes.Add(religions_traditions_Traditions);


            Category religions_spread = new Category(5, "Spread", "<i class='material-icons' translate='no'>location_on</i>", "spread");

            Attribute religions_spread_Practicing_locations = new Attribute("Practicing_locations", "Practicing Locations", "tinytext", "This field allows you to link your other Notebook.ai pages to this religion.");
            Attribute religions_spread_Practicing_races = new Attribute("Practicing_races", "Practicing Races", "varchar", "This field allows you to link your other Notebook.ai pages to this religion.");
            religions_spread.Attributes.Add(religions_spread_Practicing_locations);
            religions_spread.Attributes.Add(religions_spread_Practicing_races);


            Category religions_notes = new Category(7, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute religions_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute religions_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", "Write as little or as much as you want");
            religions_notes.Attributes.Add(religions_notes_Notes);
            religions_notes.Attributes.Add(religions_notes_Private_notes);

            Category religions_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute religions_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            religions_gallery.Attributes.Add(religions_gallery_gallery);


            content_religions.categories.Add(religions_overview);
            content_religions.categories.Add(religions_history);
            content_religions.categories.Add(religions_beliefs);
            content_religions.categories.Add(religions_traditions);
            content_religions.categories.Add(religions_spread);
            content_religions.categories.Add(religions_notes);
            content_religions.references.Add(religions_gallery);


            content_religions.categories = content_religions.categories.OrderBy(c => c.Order).ToList();
            content_religions.categories.AddAutoIncrmentId("Id");

            #endregion
            #region scenes
            Content content_scenes = new Content("scenes", true);

            Category scenes_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute scenes_overview_Name = new Attribute("Name", "Name", "varchar", "What is the name of this scene");
            Attribute scenes_overview_Summary = new Attribute("Summary", "Summary", "text", "In short what happens in this scene");
            Attribute scenes_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this scene.", "universes");
            Attribute scenes_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your scenes.");
            scenes_overview.Attributes.Add(scenes_overview_Name);
            scenes_overview.Attributes.Add(scenes_overview_Summary);
            scenes_overview.Attributes.Add(scenes_overview_Universe);
            scenes_overview.Attributes.Add(scenes_overview_Tags);


            Category scenes_members = new Category(2, "Members", "<i class='material-icons' translate='no'>face</i>", "members");

            Attribute scenes_members_Characters_in_scene = new Attribute("Characters_in_scene", "Characters In Scene", "tinytext", "This field allows you to link your other Notebook.ai pages to this scene.");
            Attribute scenes_members_Locations_in_scene = new Attribute("Locations_in_scene", "Locations In Scene", "tinytext", "This field allows you to link your other Notebook.ai pages to this scene.");
            Attribute scenes_members_Items_in_scene = new Attribute("Items_in_scene", "Items In Scene", "tinytext", "This field allows you to link your other Notebook.ai pages to this scene.");
            scenes_members.Attributes.Add(scenes_members_Characters_in_scene);
            scenes_members.Attributes.Add(scenes_members_Locations_in_scene);
            scenes_members.Attributes.Add(scenes_members_Items_in_scene);


            Category scenes_action = new Category(3, "Action", "<i class='material-icons' translate='no'>gesture</i>", "action");

            Attribute scenes_action_Description = new Attribute("Description", "Description", "text", "What happens in this scene");
            Attribute scenes_action_Results = new Attribute("Results", "Results", "text", "At the end of this scene what has changed");
            Attribute scenes_action_What_caused_this = new Attribute("What_caused_this", "What Caused This", "text", "");
            scenes_action.Attributes.Add(scenes_action_Description);
            scenes_action.Attributes.Add(scenes_action_Results);
            scenes_action.Attributes.Add(scenes_action_What_caused_this);


            Category scenes_notes = new Category(5, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute scenes_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute scenes_notes_Private_notes = new Attribute("Private_notes", "Private Notes", "text", "Write as little or as much as you want");
            scenes_notes.Attributes.Add(scenes_notes_Notes);
            scenes_notes.Attributes.Add(scenes_notes_Private_notes);

            Category scenes_gallery = new Category(4, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute scenes_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            scenes_gallery.Attributes.Add(scenes_gallery_gallery);


            content_scenes.categories.Add(scenes_overview);
            content_scenes.categories.Add(scenes_members);
            content_scenes.categories.Add(scenes_action);
            content_scenes.categories.Add(scenes_notes);
            content_scenes.references.Add(scenes_gallery);


            content_scenes.categories = content_scenes.categories.OrderBy(c => c.Order).ToList();
            content_scenes.categories.AddAutoIncrmentId("Id");

            #endregion
            #region sports
            Content content_sports = new Content("sports", true);

            Category sports_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute sports_overview_Name = new Attribute("Name", "Name", "varchar", "What is this sport named");
            Attribute sports_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this sport");
            Attribute sports_overview_Nicknames = new Attribute("Nicknames", "Nicknames", "text", "What other names is this sport known by");
            Attribute sports_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this sport.", "universes");
            Attribute sports_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your sports.");
            sports_overview.Attributes.Add(sports_overview_Name);
            sports_overview.Attributes.Add(sports_overview_Description);
            sports_overview.Attributes.Add(sports_overview_Nicknames);
            sports_overview.Attributes.Add(sports_overview_Universe);
            sports_overview.Attributes.Add(sports_overview_Tags);


            Category sports_setup = new Category(2, "Setup", "<i class='material-icons' translate='no'>input</i>", "setup");

            Attribute sports_setup_Play_area = new Attribute("Play_area", "Play Area", "varchar", "Where is this sport played What surfaces or environments are needed");
            Attribute sports_setup_Equipment = new Attribute("Equipment", "Equipment", "varchar", "What equipment do players need to play this sport");
            Attribute sports_setup_Number_of_players = new Attribute("Number_of_players", "Number Of Players", "int", "How many people can play this sport in a game How many on each team How many teams");
            Attribute sports_setup_Scoring = new Attribute("Scoring", "Scoring", "varchar", "How do players score points in this sport");
            Attribute sports_setup_Penalties = new Attribute("Penalties", "Penalties", "varchar", "How can players be penalized in this sport");
            Attribute sports_setup_How_to_win = new Attribute("How_to_win", "How To Win", "tinytext", "How does a player or team win in this sport");
            sports_setup.Attributes.Add(sports_setup_Play_area);
            sports_setup.Attributes.Add(sports_setup_Equipment);
            sports_setup.Attributes.Add(sports_setup_Number_of_players);
            sports_setup.Attributes.Add(sports_setup_Scoring);
            sports_setup.Attributes.Add(sports_setup_Penalties);
            sports_setup.Attributes.Add(sports_setup_How_to_win);


            Category sports_playing = new Category(3, "Playing", "<i class='material-icons' translate='no'>directions_run</i>", "playing");

            Attribute sports_playing_Rules = new Attribute("Rules", "Rules", "tinytext", "What are the rules to play this sport");
            Attribute sports_playing_Game_time = new Attribute("Game_time", "Game Time", "double", "How long does a game of this sport take");
            Attribute sports_playing_Positions = new Attribute("Positions", "Positions", "int", "What positions do players of this sport play");
            Attribute sports_playing_Strategies = new Attribute("Strategies", "Strategies", "varchar", "What are the most common strategies players of this sport apply");
            Attribute sports_playing_Common_injuries = new Attribute("Common_injuries", "Common Injuries", "tinytext", "How are players of this sport most commonly injured What risks are there");
            Attribute sports_playing_Most_important_muscles = new Attribute("Most_important_muscles", "Most Important Muscles", "varchar", "What muscles are most important to play this sport well");
            sports_playing.Attributes.Add(sports_playing_Rules);
            sports_playing.Attributes.Add(sports_playing_Game_time);
            sports_playing.Attributes.Add(sports_playing_Positions);
            sports_playing.Attributes.Add(sports_playing_Strategies);
            sports_playing.Attributes.Add(sports_playing_Common_injuries);
            sports_playing.Attributes.Add(sports_playing_Most_important_muscles);


            Category sports_culture = new Category(4, "Culture", "<i class='material-icons' translate='no'>group</i>", "culture");

            Attribute sports_culture_Uniforms = new Attribute("Uniforms", "Uniforms", "tinytext", "What do the uniforms of this sport look like");
            Attribute sports_culture_Merchandise = new Attribute("Merchandise", "Merchandise", "tinytext", "What merchandise is popular with this sports teams");
            Attribute sports_culture_Popularity = new Attribute("Popularity", "Popularity", "tinytext", "How popular is this sport");
            Attribute sports_culture_Players = new Attribute("Players", "Players", "tinytext", "Who plays this sport");
            Attribute sports_culture_Countries = new Attribute("Countries", "Countries", "tinytext", "Which countries play this sport");
            Attribute sports_culture_Teams = new Attribute("Teams", "Teams", "tinytext", "What teams play this sport");
            Attribute sports_culture_Traditions = new Attribute("Traditions", "Traditions", "tinytext", "What traditions are common in this sport");
            sports_culture.Attributes.Add(sports_culture_Uniforms);
            sports_culture.Attributes.Add(sports_culture_Merchandise);
            sports_culture.Attributes.Add(sports_culture_Popularity);
            sports_culture.Attributes.Add(sports_culture_Players);
            sports_culture.Attributes.Add(sports_culture_Countries);
            sports_culture.Attributes.Add(sports_culture_Teams);
            sports_culture.Attributes.Add(sports_culture_Traditions);


            Category sports_history = new Category(5, "History", "<i class='material-icons' translate='no'>history</i>", "history");

            Attribute sports_history_Origin_story = new Attribute("Origin_story", "Origin Story", "text", "How and when did this sport originate");
            Attribute sports_history_Creators = new Attribute("Creators", "Creators", "tinytext", "Who originally created this sport");
            Attribute sports_history_Evolution = new Attribute("Evolution", "Evolution", "tinytext", "How has this sport changed over time");
            Attribute sports_history_Famous_games = new Attribute("Famous_games", "Famous Games", "tinytext", "What were the most famous games of this sport in history");
            sports_history.Attributes.Add(sports_history_Origin_story);
            sports_history.Attributes.Add(sports_history_Creators);
            sports_history.Attributes.Add(sports_history_Evolution);
            sports_history.Attributes.Add(sports_history_Famous_games);


            Category sports_notes = new Category(7, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute sports_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute sports_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            sports_notes.Attributes.Add(sports_notes_Notes);
            sports_notes.Attributes.Add(sports_notes_Private_Notes);

            Category sports_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute sports_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            sports_gallery.Attributes.Add(sports_gallery_gallery);


            content_sports.categories.Add(sports_overview);
            content_sports.categories.Add(sports_setup);
            content_sports.categories.Add(sports_playing);
            content_sports.categories.Add(sports_culture);
            content_sports.categories.Add(sports_history);
            content_sports.categories.Add(sports_notes);
            content_sports.references.Add(sports_gallery);


            content_sports.categories = content_sports.categories.OrderBy(c => c.Order).ToList();
            content_sports.categories.AddAutoIncrmentId("Id");

            #endregion
            #region technologies
            Content content_technologies = new Content("technologies", true);

            Category technologies_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute technologies_overview_Name = new Attribute("Name", "Name", "varchar", "Whats the name of this technology");
            Attribute technologies_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this technology");
            Attribute technologies_overview_Other_Names = new Attribute("Other_Names", "Other Names", "varchar", "What other names is this technology known by");
            Attribute technologies_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this technology.", "universes");
            Attribute technologies_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your technologies.");
            technologies_overview.Attributes.Add(technologies_overview_Name);
            technologies_overview.Attributes.Add(technologies_overview_Description);
            technologies_overview.Attributes.Add(technologies_overview_Other_Names);
            technologies_overview.Attributes.Add(technologies_overview_Universe);
            technologies_overview.Attributes.Add(technologies_overview_Tags);


            Category technologies_production = new Category(2, "Production", "<i class='material-icons' translate='no'>build</i>", "production");

            Attribute technologies_production_Materials = new Attribute("Materials", "Materials", "tinytext", "What materials is this technology made with");
            Attribute technologies_production_Manufacturing_Process = new Attribute("Manufacturing_Process", "Manufacturing Process", "tinytext", "How is this technology produced and manufactured");
            Attribute technologies_production_Sales_Process = new Attribute("Sales_Process", "Sales Process", "varchar", "How is this technology sold");
            Attribute technologies_production_Cost = new Attribute("Cost", "Cost", "double", "How much does this technology cost");
            technologies_production.Attributes.Add(technologies_production_Materials);
            technologies_production.Attributes.Add(technologies_production_Manufacturing_Process);
            technologies_production.Attributes.Add(technologies_production_Sales_Process);
            technologies_production.Attributes.Add(technologies_production_Cost);


            Category technologies_presence = new Category(3, "Presence", "<i class='material-icons' translate='no'>blur_on</i>", "presence");

            Attribute technologies_presence_Characters = new Attribute("Characters", "Characters", "tinytext", "This field allows you to link your other Notebook.ai pages to this technology.");
            Attribute technologies_presence_Towns = new Attribute("Towns", "Towns", "varchar", "This field allows you to link your other Notebook.ai pages to this technology.");
            Attribute technologies_presence_Countries = new Attribute("Countries", "Countries", "varchar", "This field allows you to link your other Notebook.ai pages to this technology.");
            Attribute technologies_presence_Groups = new Attribute("Groups", "Groups", "varchar", "This field allows you to link your other Notebook.ai pages to this technology.");
            Attribute technologies_presence_Creatures = new Attribute("Creatures", "Creatures", "varchar", "This field allows you to link your other Notebook.ai pages to this technology.");
            Attribute technologies_presence_Planets = new Attribute("Planets", "Planets", "varchar", "This field allows you to link your other Notebook.ai pages to this technology.");
            Attribute technologies_presence_Rarity = new Attribute("Rarity", "Rarity", "varchar", "How rare or common is this technology");
            technologies_presence.Attributes.Add(technologies_presence_Characters);
            technologies_presence.Attributes.Add(technologies_presence_Towns);
            technologies_presence.Attributes.Add(technologies_presence_Countries);
            technologies_presence.Attributes.Add(technologies_presence_Groups);
            technologies_presence.Attributes.Add(technologies_presence_Creatures);
            technologies_presence.Attributes.Add(technologies_presence_Planets);
            technologies_presence.Attributes.Add(technologies_presence_Rarity);


            Category technologies_use = new Category(4, "Use", "<i class='material-icons' translate='no'>speaker_phone</i>", "use");

            Attribute technologies_use_Purpose = new Attribute("Purpose", "Purpose", "tinytext", "What is this technology used for");
            Attribute technologies_use_How_It_Works = new Attribute("How_It_Works", "How It Works", "text", "How does this technology work");
            Attribute technologies_use_Resources_Used = new Attribute("Resources_Used", "Resources Used", "tinytext", "What resources does this technology use");
            Attribute technologies_use_Magic_effects = new Attribute("Magic_effects", "Magic Effects", "tinytext", "This field allows you to link your other Notebook.ai pages to this technology.");
            technologies_use.Attributes.Add(technologies_use_Purpose);
            technologies_use.Attributes.Add(technologies_use_How_It_Works);
            technologies_use.Attributes.Add(technologies_use_Resources_Used);
            technologies_use.Attributes.Add(technologies_use_Magic_effects);


            Category technologies_appearance = new Category(5, "Appearance", "<i class='material-icons' translate='no'>sd_card</i>", "appearance");

            Attribute technologies_appearance_Physical_Description = new Attribute("Physical_Description", "Physical Description", "tinytext", "What does this technology look like");
            Attribute technologies_appearance_Size = new Attribute("Size", "Size", "double", "How big is this technology on average");
            Attribute technologies_appearance_Weight = new Attribute("Weight", "Weight", "double", "How much does this technology weigh on average");
            Attribute technologies_appearance_Colors = new Attribute("Colors", "Colors", "varchar", "What colors are this technology usually");
            technologies_appearance.Attributes.Add(technologies_appearance_Physical_Description);
            technologies_appearance.Attributes.Add(technologies_appearance_Size);
            technologies_appearance.Attributes.Add(technologies_appearance_Weight);
            technologies_appearance.Attributes.Add(technologies_appearance_Colors);


            Category technologies_related_technologies = new Category(6, "Related Technologies", "<i class='material-icons' translate='no'>call_split</i>", "related_technologies");

            Attribute technologies_related_technologies_Related_technologies = new Attribute("Related_technologies", "Related Technologies", "varchar", "");
            Attribute technologies_related_technologies_Parent_technologies = new Attribute("Parent_technologies", "Parent Technologies", "varchar", "");
            Attribute technologies_related_technologies_Child_technologies = new Attribute("Child_technologies", "Child Technologies", "varchar", "");
            technologies_related_technologies.Attributes.Add(technologies_related_technologies_Related_technologies);
            technologies_related_technologies.Attributes.Add(technologies_related_technologies_Parent_technologies);
            technologies_related_technologies.Attributes.Add(technologies_related_technologies_Child_technologies);


            Category technologies_notes = new Category(8, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute technologies_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute technologies_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            technologies_notes.Attributes.Add(technologies_notes_Notes);
            technologies_notes.Attributes.Add(technologies_notes_Private_Notes);

            Category technologies_gallery = new Category(7, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute technologies_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            technologies_gallery.Attributes.Add(technologies_gallery_gallery);


            content_technologies.categories.Add(technologies_overview);
            content_technologies.categories.Add(technologies_production);
            content_technologies.categories.Add(technologies_presence);
            content_technologies.categories.Add(technologies_use);
            content_technologies.categories.Add(technologies_appearance);
            content_technologies.categories.Add(technologies_related_technologies);
            content_technologies.categories.Add(technologies_notes);
            content_technologies.references.Add(technologies_gallery);


            content_technologies.categories = content_technologies.categories.OrderBy(c => c.Order).ToList();
            content_technologies.categories.AddAutoIncrmentId("Id");

            #endregion
            #region towns
            Content content_towns = new Content("towns", true);

            Category towns_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute towns_overview_Name = new Attribute("Name", "Name", "varchar", "Whats the name of this town");
            Attribute towns_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this town");
            Attribute towns_overview_Other_names = new Attribute("Other_names", "Other Names", "varchar", "What other names is this town known by");
            Attribute towns_overview_Country = new Attribute("Country", "Country", "int", "This field allows you to link your other Notebook.ai pages to this town.");
            Attribute towns_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this town.", "universes");
            Attribute towns_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your towns.");
            towns_overview.Attributes.Add(towns_overview_Name);
            towns_overview.Attributes.Add(towns_overview_Description);
            towns_overview.Attributes.Add(towns_overview_Other_names);
            towns_overview.Attributes.Add(towns_overview_Country);
            towns_overview.Attributes.Add(towns_overview_Universe);
            towns_overview.Attributes.Add(towns_overview_Tags);


            Category towns_populace = new Category(2, "Populace", "<i class='material-icons' translate='no'>group</i>", "populace");

            Attribute towns_populace_Citizens = new Attribute("Citizens", "Citizens", "double", "This field allows you to link your other Notebook.ai pages to this town.");
            Attribute towns_populace_Groups = new Attribute("Groups", "Groups", "varchar", "This field allows you to link your other Notebook.ai pages to this town.");
            towns_populace.Attributes.Add(towns_populace_Citizens);
            towns_populace.Attributes.Add(towns_populace_Groups);


            Category towns_layout = new Category(3, "Layout", "<i class='material-icons' translate='no'>map</i>", "layout");

            Attribute towns_layout_Buildings = new Attribute("Buildings", "Buildings", "int", "What buildings are in this town");
            Attribute towns_layout_Neighborhoods = new Attribute("Neighborhoods", "Neighborhoods", "int", "What are the neighborhoods in this town and how do they differ");
            Attribute towns_layout_Busy_areas = new Attribute("Busy_areas", "Busy Areas", "tinytext", "What are the busiest areas of this town");
            Attribute towns_layout_Landmarks = new Attribute("Landmarks", "Landmarks", "tinytext", "What landmarks are in this town");
            towns_layout.Attributes.Add(towns_layout_Buildings);
            towns_layout.Attributes.Add(towns_layout_Neighborhoods);
            towns_layout.Attributes.Add(towns_layout_Busy_areas);
            towns_layout.Attributes.Add(towns_layout_Landmarks);


            Category towns_culture = new Category(4, "Culture", "<i class='material-icons' translate='no'>face</i>", "culture");

            Attribute towns_culture_Laws = new Attribute("Laws", "Laws", "text", "What are the major laws in this town");
            Attribute towns_culture_Languages = new Attribute("Languages", "Languages", "varchar", "This field allows you to link your other Notebook.ai pages to this town.");
            Attribute towns_culture_Flora = new Attribute("Flora", "Flora", "varchar", "This field allows you to link your other Notebook.ai pages to this town.");
            Attribute towns_culture_Creatures = new Attribute("Creatures", "Creatures", "text", "This field allows you to link your other Notebook.ai pages to this town.");
            Attribute towns_culture_Politics = new Attribute("Politics", "Politics", "text", "What are the politics like in this town");
            Attribute towns_culture_Sports = new Attribute("Sports", "Sports", "tinytext", "What sports are popular in this town");
            towns_culture.Attributes.Add(towns_culture_Laws);
            towns_culture.Attributes.Add(towns_culture_Languages);
            towns_culture.Attributes.Add(towns_culture_Flora);
            towns_culture.Attributes.Add(towns_culture_Creatures);
            towns_culture.Attributes.Add(towns_culture_Politics);
            towns_culture.Attributes.Add(towns_culture_Sports);


            Category towns_history = new Category(5, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute towns_history_Founding_story = new Attribute("Founding_story", "Founding Story", "tinytext", "How was this town founded");
            Attribute towns_history_Established_year = new Attribute("Established_year", "Established Year", "int", "When was this town founded");
            towns_history.Attributes.Add(towns_history_Founding_story);
            towns_history.Attributes.Add(towns_history_Established_year);


            Category towns_sustainability = new Category(6, "Sustainability", "<i class='material-icons' translate='no'>autorenew</i>", "sustainability");

            Attribute towns_sustainability_Food_sources = new Attribute("Food_sources", "Food Sources", "tinytext", "Where does this town get their food from");
            Attribute towns_sustainability_Energy_sources = new Attribute("Energy_sources", "Energy Sources", "tinytext", "Where does this town get their energy from");
            Attribute towns_sustainability_Recycling = new Attribute("Recycling", "Recycling", "tinytext", "How are objects reused or recycled in this town");
            Attribute towns_sustainability_Waste = new Attribute("Waste", "Waste", "tinytext", "How much waste is produced in this town");
            towns_sustainability.Attributes.Add(towns_sustainability_Food_sources);
            towns_sustainability.Attributes.Add(towns_sustainability_Energy_sources);
            towns_sustainability.Attributes.Add(towns_sustainability_Recycling);
            towns_sustainability.Attributes.Add(towns_sustainability_Waste);


            Category towns_notes = new Category(8, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute towns_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute towns_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            towns_notes.Attributes.Add(towns_notes_Notes);
            towns_notes.Attributes.Add(towns_notes_Private_Notes);

            Category towns_gallery = new Category(7, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute towns_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            towns_gallery.Attributes.Add(towns_gallery_gallery);


            content_towns.categories.Add(towns_overview);
            content_towns.categories.Add(towns_populace);
            content_towns.categories.Add(towns_layout);
            content_towns.categories.Add(towns_culture);
            content_towns.categories.Add(towns_history);
            content_towns.categories.Add(towns_sustainability);
            content_towns.categories.Add(towns_notes);
            content_towns.references.Add(towns_gallery);


            content_towns.categories = content_towns.categories.OrderBy(c => c.Order).ToList();
            content_towns.categories.AddAutoIncrmentId("Id");

            #endregion
            #region traditions
            Content content_traditions = new Content("traditions", true);

            Category traditions_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute traditions_overview_Name = new Attribute("Name", "Name", "varchar", "What is this tradition named");
            Attribute traditions_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this tradition");
            Attribute traditions_overview_Type_of_tradition = new Attribute("Type_of_tradition", "Type Of Tradition", "varchar", "What kind of tradition is this tradition");
            Attribute traditions_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this tradition.", "universes");
            Attribute traditions_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "tinytext", "What other names is this tradition known by");
            Attribute traditions_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your traditions.");
            traditions_overview.Attributes.Add(traditions_overview_Name);
            traditions_overview.Attributes.Add(traditions_overview_Description);
            traditions_overview.Attributes.Add(traditions_overview_Type_of_tradition);
            traditions_overview.Attributes.Add(traditions_overview_Universe);
            traditions_overview.Attributes.Add(traditions_overview_Alternate_names);
            traditions_overview.Attributes.Add(traditions_overview_Tags);


            Category traditions_observance = new Category(2, "Observance", "<i class='material-icons' translate='no'>track_changes</i>", "observance");

            Attribute traditions_observance_Dates = new Attribute("Dates", "Dates", "text", "");
            Attribute traditions_observance_Countries = new Attribute("Countries", "Countries", "varchar", "What countries celebrate this tradition");
            Attribute traditions_observance_Groups = new Attribute("Groups", "Groups", "varchar", "What groups celebrate this tradition");
            Attribute traditions_observance_Towns = new Attribute("Towns", "Towns", "varchar", "What towns celebrate this tradition");
            traditions_observance.Attributes.Add(traditions_observance_Dates);
            traditions_observance.Attributes.Add(traditions_observance_Countries);
            traditions_observance.Attributes.Add(traditions_observance_Groups);
            traditions_observance.Attributes.Add(traditions_observance_Towns);


            Category traditions_celebrations = new Category(3, "Celebrations", "<i class='material-icons' translate='no'>flare</i>", "celebrations");

            Attribute traditions_celebrations_Activities = new Attribute("Activities", "Activities", "varchar", "What activities are common when celebrating this tradition");
            Attribute traditions_celebrations_Gifts = new Attribute("Gifts", "Gifts", "varchar", "What kinds of gifts are exchanged during this tradition");
            Attribute traditions_celebrations_Food = new Attribute("Food", "Food", "varchar", "What food is common during this tradition");
            Attribute traditions_celebrations_Symbolism = new Attribute("Symbolism", "Symbolism", "varchar", "What does this tradition symbolize in your world");
            Attribute traditions_celebrations_Games = new Attribute("Games", "Games", "varchar", "What games are played during this tradition");
            traditions_celebrations.Attributes.Add(traditions_celebrations_Activities);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Gifts);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Food);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Symbolism);
            traditions_celebrations.Attributes.Add(traditions_celebrations_Games);


            Category traditions_history = new Category(4, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute traditions_history_Etymology = new Attribute("Etymology", "Etymology", "varchar", "Where did the name for this tradition originally come from");
            Attribute traditions_history_Origin = new Attribute("Origin", "Origin", "text", "When did this tradition originate");
            Attribute traditions_history_Significance = new Attribute("Significance", "Significance", "varchar", "How important is this tradition Who is it most important to");
            Attribute traditions_history_Religions = new Attribute("Religions", "Religions", "varchar", "Which religions are associated with this tradition");
            Attribute traditions_history_Notable_events = new Attribute("Notable_events", "Notable Events", "text", "What notable events throughout history are related to this tradition");
            traditions_history.Attributes.Add(traditions_history_Etymology);
            traditions_history.Attributes.Add(traditions_history_Origin);
            traditions_history.Attributes.Add(traditions_history_Significance);
            traditions_history.Attributes.Add(traditions_history_Religions);
            traditions_history.Attributes.Add(traditions_history_Notable_events);


            Category traditions_notes = new Category(6, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute traditions_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute traditions_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            traditions_notes.Attributes.Add(traditions_notes_Notes);
            traditions_notes.Attributes.Add(traditions_notes_Private_Notes);

            Category traditions_gallery = new Category(5, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute traditions_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            traditions_gallery.Attributes.Add(traditions_gallery_gallery);


            content_traditions.categories.Add(traditions_overview);
            content_traditions.categories.Add(traditions_observance);
            content_traditions.categories.Add(traditions_celebrations);
            content_traditions.categories.Add(traditions_history);
            content_traditions.categories.Add(traditions_notes);
            content_traditions.references.Add(traditions_gallery);


            content_traditions.categories = content_traditions.categories.OrderBy(c => c.Order).ToList();
            content_traditions.categories.AddAutoIncrmentId("Id");

            #endregion
            #region universes
            Content content_universes = new Content("universes", true);

            Category universes_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute universes_overview_name = new Attribute("name", "Name", "varchar", "What is this universes name");
            Attribute universes_overview_description = new Attribute("description", "Description", "longblob", "How would you describe this universe");
            Attribute universes_overview_genre = new Attribute("genre", "Genre", "varchar", "What genre best describes this universe");
            Attribute universes_overview_privacy = new Attribute("privacy", "Privacy", "tinyint", "");
            Attribute universes_overview_favorite = new Attribute("favorite", "Favorite", "tinyint", "");
            universes_overview.Attributes.Add(universes_overview_name);
            universes_overview.Attributes.Add(universes_overview_description);
            universes_overview.Attributes.Add(universes_overview_genre);
            universes_overview.Attributes.Add(universes_overview_privacy);
            universes_overview.Attributes.Add(universes_overview_favorite);


            Category universes_history = new Category(2, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute universes_history_history = new Attribute("history", "History", "longblob", "What is this universes history");
            universes_history.Attributes.Add(universes_history_history);


            Category universes_rules = new Category(3, "Rules", "<i class='material-icons' translate='no'>gavel</i>", "rules");

            Attribute universes_rules_laws_of_physics = new Attribute("laws_of_physics", "Laws Of Physics", "text", "");
            Attribute universes_rules_magic_system = new Attribute("magic_system", "Magic System", "text", "");
            Attribute universes_rules_technology = new Attribute("technology", "Technology", "text", "");
            universes_rules.Attributes.Add(universes_rules_laws_of_physics);
            universes_rules.Attributes.Add(universes_rules_magic_system);
            universes_rules.Attributes.Add(universes_rules_technology);


            Category universes_notes = new Category(4, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute universes_notes_notes = new Attribute("notes", "Notes", "longblob", "Write as little or as much as you want");
            Attribute universes_notes_private_notes = new Attribute("private_notes", "Private Notes", "text", "Write as little or as much as you want");
            universes_notes.Attributes.Add(universes_notes_notes);
            universes_notes.Attributes.Add(universes_notes_private_notes);

            Category universes_gallery = new Category(5, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute universes_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            universes_gallery.Attributes.Add(universes_gallery_gallery);


            content_universes.categories.Add(universes_overview);
            content_universes.categories.Add(universes_history);
            content_universes.categories.Add(universes_rules);
            content_universes.categories.Add(universes_notes);
            content_universes.references.Add(universes_gallery);


            content_universes.categories = content_universes.categories.OrderBy(c => c.Order).ToList();
            content_universes.categories.AddAutoIncrmentId("Id");

            #endregion
            #region vehicles
            Content content_vehicles = new Content("vehicles", true);

            Category vehicles_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute vehicles_overview_Name = new Attribute("Name", "Name", "varchar", "What is the name of this vehicle");
            Attribute vehicles_overview_Description = new Attribute("Description", "Description", "text", "How would you describe this vehicle");
            Attribute vehicles_overview_Universe = new Attribute("Universe", "Universe", "bigint", "This field allows you to link your other Notebook.ai pages to this vehicle.", "universes");
            Attribute vehicles_overview_Type_of_vehicle = new Attribute("Type_of_vehicle", "Type Of Vehicle", "text", "What kind of vehicle is this vehicle");
            Attribute vehicles_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "text", "What other names do people call this vehicle");
            Attribute vehicles_overview_Tags = new Attribute("Tags", "Tags", "varchar", "This field lets you add clickable tags to your vehicles.");
            vehicles_overview.Attributes.Add(vehicles_overview_Name);
            vehicles_overview.Attributes.Add(vehicles_overview_Description);
            vehicles_overview.Attributes.Add(vehicles_overview_Universe);
            vehicles_overview.Attributes.Add(vehicles_overview_Type_of_vehicle);
            vehicles_overview.Attributes.Add(vehicles_overview_Alternate_names);
            vehicles_overview.Attributes.Add(vehicles_overview_Tags);


            Category vehicles_looks = new Category(2, "Looks", "<i class='material-icons' translate='no'>airport_shuttle</i>", "looks");

            Attribute vehicles_looks_Size = new Attribute("Size", "Size", "double", "About how big or small is this vehicle");
            Attribute vehicles_looks_Doors = new Attribute("Doors", "Doors", "int", "How many doors does this vehicle have");
            Attribute vehicles_looks_Materials = new Attribute("Materials", "Materials", "text", "What is this vehicle made of");
            Attribute vehicles_looks_Dimensions = new Attribute("Dimensions", "Dimensions", "double", "What are the dimensions of this vehicle");
            Attribute vehicles_looks_Windows = new Attribute("Windows", "Windows", "int", "How many windows does this vehicle have");
            Attribute vehicles_looks_Colors = new Attribute("Colors", "Colors", "varchar", "What colors does this vehicle come in");
            Attribute vehicles_looks_Designer = new Attribute("Designer", "Designer", "varchar", "Who designed this vehicle");
            vehicles_looks.Attributes.Add(vehicles_looks_Size);
            vehicles_looks.Attributes.Add(vehicles_looks_Doors);
            vehicles_looks.Attributes.Add(vehicles_looks_Materials);
            vehicles_looks.Attributes.Add(vehicles_looks_Dimensions);
            vehicles_looks.Attributes.Add(vehicles_looks_Windows);
            vehicles_looks.Attributes.Add(vehicles_looks_Colors);
            vehicles_looks.Attributes.Add(vehicles_looks_Designer);


            Category vehicles_travel = new Category(3, "Travel", "<i class='material-icons' translate='no'>near_me</i>", "travel");

            Attribute vehicles_travel_Speed = new Attribute("Speed", "Speed", "double", "How fast does this vehicle go What is its top speed");
            Attribute vehicles_travel_Distance = new Attribute("Distance", "Distance", "varchar", "How far can this vehicle go");
            Attribute vehicles_travel_Fuel = new Attribute("Fuel", "Fuel", "double", "What kind of fuel does this vehicle use How does it refill");
            Attribute vehicles_travel_Safety = new Attribute("Safety", "Safety", "text", "How safe is it to operate this vehicle");
            Attribute vehicles_travel_Features = new Attribute("Features", "Features", "text", "What features are available in this vehicle");
            vehicles_travel.Attributes.Add(vehicles_travel_Speed);
            vehicles_travel.Attributes.Add(vehicles_travel_Distance);
            vehicles_travel.Attributes.Add(vehicles_travel_Fuel);
            vehicles_travel.Attributes.Add(vehicles_travel_Safety);
            vehicles_travel.Attributes.Add(vehicles_travel_Features);


            Category vehicles_manufacturing = new Category(4, "Manufacturing", "<i class='material-icons' translate='no'>gavel</i>", "manufacturing");

            Attribute vehicles_manufacturing_Manufacturer = new Attribute("Manufacturer", "Manufacturer", "varchar", "Who manufactures this vehicle");
            Attribute vehicles_manufacturing_Costs = new Attribute("Costs", "Costs", "double", "How much does it cost to bulid this vehicle");
            Attribute vehicles_manufacturing_Weight = new Attribute("Weight", "Weight", "double", "How much does this vehicle weigh");
            Attribute vehicles_manufacturing_Country = new Attribute("Country", "Country", "int", "What countries manufacture this vehicle");
            Attribute vehicles_manufacturing_Variants = new Attribute("Variants", "Variants", "tinytext", "What alternate styles or makes do this vehicle come in");
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Manufacturer);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Costs);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Weight);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Country);
            vehicles_manufacturing.Attributes.Add(vehicles_manufacturing_Variants);


            Category vehicles_operators = new Category(5, "Operators", "<i class='material-icons' translate='no'>people_outline</i>", "operators");

            Attribute vehicles_operators_Owner = new Attribute("Owner", "Owner", "varchar", "Who owns this vehicle");
            vehicles_operators.Attributes.Add(vehicles_operators_Owner);


            Category vehicles_notes = new Category(7, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute vehicles_notes_Notes = new Attribute("Notes", "Notes", "text", "Write as little or as much as you want");
            Attribute vehicles_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "Write as little or as much as you want");
            vehicles_notes.Attributes.Add(vehicles_notes_Notes);
            vehicles_notes.Attributes.Add(vehicles_notes_Private_Notes);

            Category vehicles_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute vehicles_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            vehicles_gallery.Attributes.Add(vehicles_gallery_gallery);


            content_vehicles.categories.Add(vehicles_overview);
            content_vehicles.categories.Add(vehicles_looks);
            content_vehicles.categories.Add(vehicles_travel);
            content_vehicles.categories.Add(vehicles_manufacturing);
            content_vehicles.categories.Add(vehicles_operators);
            content_vehicles.categories.Add(vehicles_notes);
            content_vehicles.references.Add(vehicles_gallery);


            content_vehicles.categories = content_vehicles.categories.OrderBy(c => c.Order).ToList();
            content_vehicles.categories.AddAutoIncrmentId("Id");

            #endregion
            #region organizations
            Content content_organizations = new Content("organizations", true);

            Category organizations_overview = new Category(1, "Overview", "<i class='material-icons' translate='no'>info</i>", "overview");

            Attribute organizations_overview_Name = new Attribute("Name", "Name", "varchar", "What how when");
            Attribute organizations_overview_Alternate_names = new Attribute("Alternate_names", "Alternate Names", "tinytext", "What how when");
            Attribute organizations_overview_Description = new Attribute("Description", "Description", "text", "What how when");
            Attribute organizations_overview_Universe = new Attribute("Universe", "Universe", "bigint", "What how when", "universes");
            Attribute organizations_overview_Tags = new Attribute("Tags", "Tags", "varchar", "What how when");
            organizations_overview.Attributes.Add(organizations_overview_Name);
            organizations_overview.Attributes.Add(organizations_overview_Alternate_names);
            organizations_overview.Attributes.Add(organizations_overview_Description);
            organizations_overview.Attributes.Add(organizations_overview_Universe);
            organizations_overview.Attributes.Add(organizations_overview_Tags);


            Category organizations_structure = new Category(2, "Structure", "<i class='material-icons' translate='no'>account_tree</i>", "structure");

            Attribute organizations_structure_Type_of_organization = new Attribute("Type_of_organization", "Type of Organization", "tinytext", "What how when");
            organizations_structure.Attributes.Add(organizations_structure_Type_of_organization);


            Category organizations_purpose = new Category(3, "Purpose", "<i class='material-icons' translate='no'>business</i>", "purpose");

            Attribute organizations_purpose_Purpose = new Attribute("Purpose", "Purpose", "tinytext", "What how when");
            Attribute organizations_purpose_Services = new Attribute("Services", "Services", "tinytext", "What how when");
            organizations_purpose.Attributes.Add(organizations_purpose_Purpose);
            organizations_purpose.Attributes.Add(organizations_purpose_Services);


            Category organizations_members = new Category(4, "Members", "<i class='material-icons' translate='no'>list</i>", "members");

            Attribute organizations_members_Owner = new Attribute("Owner", "Owner", "mediumtext", "What how when");
            Attribute organizations_members_Members = new Attribute("Members", "Members", "text", "What how when");
            organizations_members.Attributes.Add(organizations_members_Owner);
            organizations_members.Attributes.Add(organizations_members_Members);


            Category organizations_locations = new Category(5, "Locations", "<i class='material-icons' translate='no'>location_on</i>", "locations");

            Attribute organizations_locations_Address = new Attribute("Address", "Address", "tinytext", "What how when");
            Attribute organizations_locations_Offices = new Attribute("Offices", "Offices", "tinytext", "What how when");
            Attribute organizations_locations_Locations = new Attribute("Locations", "Locations", "tinytext", "What how when");
            Attribute organizations_locations_Headquarters = new Attribute("Headquarters", "Headquarters", "tinytext", "What how when");
            organizations_locations.Attributes.Add(organizations_locations_Address);
            organizations_locations.Attributes.Add(organizations_locations_Offices);
            organizations_locations.Attributes.Add(organizations_locations_Locations);
            organizations_locations.Attributes.Add(organizations_locations_Headquarters);


            Category organizations_hierarchy = new Category(6, "Hierarchy", "<i class='material-icons' translate='no'>call_split</i>", "hierarchy");

            Attribute organizations_hierarchy_Sub_organizations = new Attribute("Sub_organizations", "Sub Organizations", "tinytext", "What how when");
            Attribute organizations_hierarchy_Super_organizations = new Attribute("Super_organizations", "Super Organizations", "tinytext", "What how when");
            Attribute organizations_hierarchy_Sister_organizations = new Attribute("Sister_organizations", "Sister Organizations", "tinytext", "What how when");
            Attribute organizations_hierarchy_Organization_structure = new Attribute("Organization_structure", "Organization Structure", "tinytext", "What how when");
            organizations_hierarchy.Attributes.Add(organizations_hierarchy_Sub_organizations);
            organizations_hierarchy.Attributes.Add(organizations_hierarchy_Super_organizations);
            organizations_hierarchy.Attributes.Add(organizations_hierarchy_Sister_organizations);
            organizations_hierarchy.Attributes.Add(organizations_hierarchy_Organization_structure);


            Category organizations_history = new Category(7, "History", "<i class='material-icons' translate='no'>date_range</i>", "history");

            Attribute organizations_history_Formation_year = new Attribute("Formation_year", "Formation Year", "int", "What how when");
            Attribute organizations_history_Closure_year = new Attribute("Closure_year", "Closure Year", "int", "What how when");
            Attribute organizations_history_Rival_organizations = new Attribute("Rival_organizations", "Rival Organizations", "varchar", "What how when");
            organizations_history.Attributes.Add(organizations_history_Formation_year);
            organizations_history.Attributes.Add(organizations_history_Closure_year);
            organizations_history.Attributes.Add(organizations_history_Rival_organizations);


            Category organizations_notes = new Category(8, "Notes", "<i class='material-icons' translate='no'>edit</i>", "notes");

            Attribute organizations_notes_Notes = new Attribute("Notes", "Notes", "longtext", "What how when");
            Attribute organizations_notes_Private_Notes = new Attribute("Private_Notes", "Private Notes", "text", "What how when");
            organizations_notes.Attributes.Add(organizations_notes_Notes);
            organizations_notes.Attributes.Add(organizations_notes_Private_Notes);

            Category organizations_gallery = new Category(6, "Gallery", "<i class='material-icons' translate='no'>photo_library</i>", "gallery");

            Attribute organizations_gallery_gallery = new Attribute("Gallery", "Gallery", "blob", "Upload more images", false, true);
            organizations_gallery.Attributes.Add(organizations_gallery_gallery);


            content_organizations.categories.Add(organizations_overview);
            content_organizations.categories.Add(organizations_structure);
            content_organizations.categories.Add(organizations_purpose);
            content_organizations.categories.Add(organizations_members);
            content_organizations.categories.Add(organizations_locations);
            content_organizations.categories.Add(organizations_hierarchy);
            content_organizations.categories.Add(organizations_history);
            content_organizations.categories.Add(organizations_notes);
            content_organizations.references.Add(organizations_gallery);


            content_organizations.categories = content_organizations.categories.OrderBy(c => c.Order).ToList();
            content_organizations.categories.AddAutoIncrmentId("Id");

            #endregion
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
            contentTemplate.Contents.Add(content_organizations);


            #endregion


            var jsonTemplate = JsonConvert.SerializeObject(contentTemplate, Formatting.Indented);
            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            "my_world_new", "Templates_MOD", "Content_Template" + DateTime.Now.ToShortDateString() + ".json"), jsonTemplate, true);
        }

        void GenerateCodeFromJSON()
        {

            string filePath = @"C:\Users\Sandeep\Desktop\my_world_new\Templates_MOD\Content_Template21-10-2022.json";

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
                string content_object_name = "content_" + contentType.content_type;
                sb.AppendLine("#region " + contentType.content_type);
                sb.AppendLine("Content " + content_object_name + " = new Content(\"" + contentType.content_type + "\", true);");
                sbContentADD.AppendLine("contentTemplate.Contents.Add(content_" + contentType.content_type + ");");

                StringBuilder sbCatADD = new StringBuilder();

                foreach (var cat in contentType.categories)
                {
                    sb.AppendLine();
                    if (cat.Name.ToLower() != "gallery")
                    {
                        sb.AppendLine("Category " + contentType.content_type + "_" + cat.Name + " = new Category(" + cat.Order + ", \"" + cat.Label + "\", \"" + cat.Icon + "\", \"" + cat.Name + "\");");
                        sbCatADD.AppendLine("content_" + contentType.content_type + ".categories.Add(" + contentType.content_type + "_" + cat.Name + ");");

                        sb.AppendLine();
                        StringBuilder sbCatAttrADD = new StringBuilder();
                        foreach (var att in cat.Attributes)
                        {
                            if (att.field_name.ToLower() == "universe")
                                sb.AppendLine("Attribute " + contentType.content_type + "_" + cat.Name + "_" + att.field_name + " = new Attribute(\"" + att.field_name + "\", \"" + att.field_label + "\", \"" + att.field_type + "\", " + "\"" + att.help_text + "\", \"universes\");");
                            else
                                sb.AppendLine("Attribute " + contentType.content_type + "_" + cat.Name + "_" + att.field_name + " = new Attribute(\"" + att.field_name + "\", \"" + att.field_label + "\", \"" + att.field_type + "\", " + "\"" + att.help_text + "\");");
                            sbCatAttrADD.AppendLine("" + contentType.content_type + "_" + cat.Name + ".Attributes.Add(" + contentType.content_type + "_" + cat.Name + "_" + att.field_name + ");");
                        }

                        sb.AppendLine(sbCatAttrADD.ToString());

                    }
                    else
                    {
                        sb.AppendLine("Category " + contentType.content_type + "_" + cat.Name + " = new Category(" + cat.Order + ", \"" + cat.Label + "\", \"" + cat.Icon + "\", \"" + cat.Name + "\");");

                        sb.AppendLine();
                        StringBuilder sbCatAttrADD = new StringBuilder();

                        sb.AppendLine("Attribute " + contentType.content_type + "_" + cat.Name + "_gallery = new Attribute(\"Gallery\", \"Gallery\", \"blob\", \"Upload more images\");");

                        sbCatAttrADD.AppendLine("" + contentType.content_type + "_" + cat.Name + ".Attributes.Add(" + contentType.content_type + "_" + cat.Name + "_gallery);");

                        sb.AppendLine(sbCatAttrADD.ToString());
                        sbCatADD.AppendLine("content_" + contentType.content_type + ".references.Add(" + contentType.content_type + "_" + cat.Name + ");");


                    }
                }
                foreach (var cat in contentType.references)
                {
                    sb.AppendLine("Category " + contentType.content_type + "_" + cat.Name + " = new Category(" + cat.Order + ", \"" + cat.Label + "\", \"" + cat.Icon + "\", \"" + cat.Name + "\");");

                    sb.AppendLine();
                    StringBuilder sbCatAttrADD = new StringBuilder();

                    sb.AppendLine("Attribute " + contentType.content_type + "_" + cat.Name + "_gallery = new Attribute(\"Gallery\", \"Gallery\", \"blob\", \"Upload more images\", false, true);");
                    sbCatAttrADD.AppendLine("" + contentType.content_type + "_" + cat.Name + ".Attributes.Add(" + contentType.content_type + "_" + cat.Name + "_gallery);");

                    sb.AppendLine(sbCatAttrADD.ToString());
                    sbCatADD.AppendLine("content_" + contentType.content_type + ".references.Add(" + contentType.content_type + "_" + cat.Name + ");");

                }
                sb.AppendLine();
                sb.AppendLine(sbCatADD.ToString());
                sb.AppendLine();
                sb.AppendLine(content_object_name + ".categories = " + content_object_name + ".categories.OrderBy(c=>c.Order).ToList();");
                sb.AppendLine(content_object_name + ".categories.AddAutoIncrmentId(\"Id\");");
                sb.AppendLine();
                sb.AppendLine("#endregion");
            }
            sb.AppendLine(sbContentADD.ToString());
            sb.AppendLine();
            sb.AppendLine("#endregion");
            //var jsonTemplate = JsonConvert.SerializeObject(contentTemplate, Formatting.Indented);
            //FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            //"my_book", "Templates_MOD", "Content_Template.json"), jsonTemplate, true);

            FileUtility.SaveDataToFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            "my_world_new", "Content_Template.cs"), sb.ToString(), true);

        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            DateTime dtToday = DateTime.Now;
            DateTime dtTomorrow = DateTime.Now.AddDays(1);
            DateTime dtYesterday = DateTime.Now.AddDays(-1);
            DateTime dtThisWeek = DateTime.Now.AddDays(2);
            DateTime dtThisMonth = DateTime.Now.AddDays(12);
            DateTime dtThisYear = DateTime.Now.AddMonths(5);

            rtbResult.AppendText(dtToday.ToShortDateString() + " is " + dtToday.GetDateString() + Environment.NewLine);
            rtbResult.AppendText(dtTomorrow.ToShortDateString() + " is " + dtTomorrow.GetDateString() + Environment.NewLine);
            rtbResult.AppendText(dtYesterday.ToShortDateString() + " is " + dtYesterday.GetDateString() + Environment.NewLine);
            rtbResult.AppendText(dtThisWeek.ToShortDateString() + " is " + dtThisWeek.GetDateString() + Environment.NewLine);
            rtbResult.AppendText(dtThisMonth.ToShortDateString() + " is " + dtThisMonth.GetDateString() + Environment.NewLine);
            rtbResult.AppendText(dtThisYear.ToShortDateString() + " is " + dtThisYear.GetDateString() + Environment.NewLine);
        }
    }
}
