using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddLegacyHospitalsAndTheirServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLegacy",
                table: "TbService",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "TbService",
                columns: new[] { "Code", "IsLegacy", "Name", "PHECCode", "ServiceAdGroup" },
                values: new object[,]
                {
                    { "TBS0424", true, "Accrington Victoria Hospital", "E45000018", null },
                    { "TBS0790", true, "Rushden Memorial Clinic", "E45000016", null },
                    { "TBS0791", true, "Ruth Lancaster James Hospital (Alston Maternity)", "E45000018", null },
                    { "TBS0792", true, "Rutson Hospital", "E45000010", null },
                    { "TBS0793", true, "Rye Memorial Hospital", "E45000019", null },
                    { "TBS0794", true, "Ryhope General Hospital", "E45000009", null },
                    { "TBS0795", true, "Saffron Walden Community Hospital", "E45000017", null },
                    { "TBS0789", true, "Runwell Hospital", "E45000017", null },
                    { "TBS0796", true, "Samaritan Hospital For Women", "E45000001", null },
                    { "TBS0798", true, "Sarum Road Hospital", "E45000019", null },
                    { "TBS0799", true, "Savernake Hospital", "E45000020", null },
                    { "TBS0800", true, "Saxon Clinic", "E45000017", null },
                    { "TBS0801", true, "Scalebor Park Hospital", "E45000010", null },
                    { "TBS0802", true, "Scarsdale Hospital", "E45000016", null },
                    { "TBS0803", true, "Scott Hospital", "E45000020", null },
                    { "TBS0797", true, "Sandringham Hospital", "E45000017", null },
                    { "TBS0804", true, "Seaford Day Hospital", "E45000019", null },
                    { "TBS0788", true, "Royston Hospital", "E45000017", null },
                    { "TBS0786", true, "Royal National Orthopaedic Hospital (Bolsover Street)", "E45000001", null },
                    { "TBS0775", true, "Ross Community Hospital", "E45000005", null },
                    { "TBS0994", true, "Rossendale Hospital", "E45000018", null },
                    { "TBS0776", true, "Rothbury Community Hospital", "E45000009", null },
                    { "TBS0777", true, "Roundway Hospital", "E45000020", null },
                    { "TBS0778", true, "Rowley Hospital", "E45000005", null },
                    { "TBS0779", true, "Rowley Regis Hospital", "E45000005", null },
                    { "TBS0787", true, "Royal National, Throat Nose & Ear Hospital", "E45000001", null },
                    { "TBS0780", true, "Roxbourne Hospital", "E45000001", null },
                    { "TBS0781", true, "Royal Eye Infirmary", "E45000020", null },
                    { "TBS0782", true, "Royal Infirmary Of Edinburgh", "PHECSCOT", null },
                    { "TBS0783", true, "Royal Leamington Spa Rehabilitation Hospital", "E45000005", null },
                    { "TBS0784", true, "Royal London Homeopathic Hospital", "E45000001", null },
                    { "TBS0996", true, "Royal Marsden Hospital (Surrey)", "E45000001", null },
                    { "TBS0785", true, "Royal National Hospital For Rheumatic Diseases", "E45000020", null },
                    { "TBS0995", true, "Royal Alexandra Children's Hospital [Brighton]", "E45000019", null },
                    { "TBS0774", true, "Rosie Hospital", "E45000017", null },
                    { "TBS0805", true, "Selby & District War Memorial Hospital", "E45000010", null },
                    { "TBS0807", true, "Shelburne Hospital", "E45000019", null },
                    { "TBS0824", true, "Spire Bristol Health Clinic", "E45000020", null },
                    { "TBS0825", true, "Spire Bushey Hospital", "E45000017", null },
                    { "TBS0826", true, "Spire Cheshire Hospital", "E45000018", null },
                    { "TBS0827", true, "Spire Clare Park Hospital", "E45000019", null },
                    { "TBS0828", true, "Spire Dunedin Hospital", "E45000019", null },
                    { "TBS0829", true, "Spire Elland Hospital", "E45000010", null },
                    { "TBS0823", true, "Spire Barnsley Consulting Rooms", "E45000010", null },
                    { "TBS0830", true, "Spire Fylde Coast Hospital", "E45000018", null },
                    { "TBS0832", true, "Spire Hull And East Riding Hospital", "E45000010", null },
                    { "TBS0833", true, "Spire Leeds Hospital", "E45000010", null },
                    { "TBS0834", true, "Spire Leicester Hospital", "E45000016", null },
                    { "TBS0835", true, "Spire Little Aston Hospital", "E45000005", null },
                    { "TBS0836", true, "Spire Livingston Clinic", "PHECSCOT", null },
                    { "TBS0837", true, "Spire Methley Park Hospital", "E45000010", null },
                    { "TBS0831", true, "Spire Gatwick Park Hospital", "E45000019", null },
                    { "TBS0806", true, "Sevenoaks Hospital", "E45000019", null },
                    { "TBS0822", true, "Southwold Hospital", "E45000017", null },
                    { "TBS0998", true, "Southlands Hospital", "E45000019", null },
                    { "TBS0808", true, "Sheppey Community Hospital", "E45000019", null },
                    { "TBS0809", true, "Shepton Mallet Community Hospital", "E45000020", null },
                    { "TBS0810", true, "Shipley Hospital", "E45000010", null },
                    { "TBS0811", true, "Shirehill Hospital", "E45000018", null },
                    { "TBS0812", true, "Shirley Oaks Hospital", "E45000001", null },
                    { "TBS0813", true, "Sir Alfred Jones Memorial Hospital (Acute)", "E45000018", null },
                    { "TBS0821", true, "Southport General Infirmary", "E45000018", null },
                    { "TBS0814", true, "Sittingbourne Memorial Hospital", "E45000019", null },
                    { "TBS0815", true, "Skipton General Hospital", "E45000010", null },
                    { "TBS0816", true, "South Hams Hospital", "E45000020", null },
                    { "TBS0817", true, "South Molton Hospital", "E45000020", null },
                    { "TBS0818", true, "South Moor Hospital", "E45000009", null },
                    { "TBS0819", true, "South Petherton Hospital", "E45000020", null },
                    { "TBS0820", true, "South Shore Hospital", "E45000018", null },
                    { "TBS0997", true, "Skegness & District General Hospital", "E45000016", null },
                    { "TBS0838", true, "Spire Norwich Hospital", "E45000017", null },
                    { "TBS0773", true, "Romsey Hospital", "E45000019", null },
                    { "TBS0771", true, "Rochford Community Hospital", "E45000017", null },
                    { "TBS0722", true, "Oxford Clinic", "E45000019", null },
                    { "TBS0723", true, "Oxted & Limpsfield Hospital", "E45000019", null },
                    { "TBS0724", true, "Paddocks Clinic", "E45000019", null },
                    { "TBS0725", true, "Paignton Hospital", "E45000020", null },
                    { "TBS0726", true, "Palmer Community Hospital", "E45000009", null },
                    { "TBS0727", true, "Park Hill Hospital", "E45000010", null },
                    { "TBS0721", true, "Ottery St Mary Hospital", "E45000020", null },
                    { "TBS0728", true, "Park Hospital [Nottingham]", "E45000016", null },
                    { "TBS0730", true, "Park Lane Medical Centre", "E45000001", null },
                    { "TBS0731", true, "Park View Clinic", "E45000001", null },
                    { "TBS0732", true, "Park View Day Hospital", "E45000018", null },
                    { "TBS0733", true, "Parklands Hospital", "E45000019", null },
                    { "TBS0734", true, "Parkwood Hospital", "E45000018", null },
                    { "TBS0735", true, "Patrick Stead Hospital", "E45000017", null },
                    { "TBS0729", true, "Park Hospital [Oxford]", "E45000019", null },
                    { "TBS0736", true, "Paulton Memorial Hospital", "E45000020", null },
                    { "TBS0720", true, "Old Cottage Hospital", "E45000019", null },
                    { "TBS0718", true, "Nuffield Health Wolverhampton Hospital", "E45000005", null },
                    { "TBS0704", true, "Nuffield Health Brighton Hospital", "E45000019", null },
                    { "TBS0705", true, "Nuffield Health Bristol Hospital", "E45000020", null },
                    { "TBS0706", true, "Nuffield Health Cheltenham Hospital", "E45000020", null },
                    { "TBS0707", true, "Nuffield Health Exeter Hospital", "E45000020", null },
                    { "TBS0708", true, "Nuffield Health Hampshire Hospital", "E45000019", null },
                    { "TBS0709", true, "Nuffield Health Ipswich Hospital", "E45000017", null },
                    { "TBS0719", true, "Okehampton Community Hospital", "E45000020", null },
                    { "TBS0710", true, "Nuffield Health Leicester Hospital", "E45000016", null },
                    { "TBS0712", true, "Nuffield Health North Staffordshire Hospital", "E45000005", null },
                    { "TBS0713", true, "Nuffield Health Plymouth Hospital", "E45000020", null },
                    { "TBS0714", true, "Nuffield Health Taunton Hospital", "E45000020", null },
                    { "TBS0715", true, "Nuffield Health Tees Hospital", "E45000009", null },
                    { "TBS0716", true, "Nuffield Health The Grosvenor Hospital Chester", "E45000018", null },
                    { "TBS0717", true, "Nuffield Health The Manor Hospital Oxford", "E45000019", null },
                    { "TBS0711", true, "Nuffield Health Newcastle-Upon-Tyne Hospital", "E45000009", null },
                    { "TBS0772", true, "Rockwell Day Hospital", "E45000020", null },
                    { "TBS0993", true, "Pemberton Clinic", "E45000018", null },
                    { "TBS0738", true, "Peterlee Community Hospital", "E45000009", null },
                    { "TBS0757", true, "Rampton Hospital", "E45000016", null },
                    { "TBS0758", true, "Ramsbottom Cottage Hospital", "E45000018", null },
                    { "TBS0759", true, "Rathbone Hospital", "E45000018", null },
                    { "TBS0760", true, "Ravenscourt Park Hospital", "E45000001", null },
                    { "TBS0761", true, "Redcliffe Day Hospital", "E45000016", null },
                    { "TBS0762", true, "Renacres Hospital", "E45000018", null },
                    { "TBS0756", true, "Radford Health Centre", "E45000016", null },
                    { "TBS0763", true, "Ribbleton Hospital", "E45000018", null },
                    { "TBS0765", true, "Ridge Lea Hospital", "E45000018", null },
                    { "TBS0766", true, "Ridley Day Hospital", "E45000020", null },
                    { "TBS0767", true, "Ripon And District Community Hospital", "E45000010", null },
                    { "TBS0768", true, "Rivers Hospital", "E45000017", null },
                    { "TBS0769", true, "Robert Jones & Agnes Hunt", "E45000005", null },
                    { "TBS0770", true, "Roborough Day Hospital", "E45000019", null },
                    { "TBS0764", true, "Richardson Hospital", "E45000009", null },
                    { "TBS0737", true, "Penrith Hospital", "E45000018", null },
                    { "TBS0755", true, "Queen Victoria Memorial Hospital", "E45000019", null },
                    { "TBS0753", true, "Queen Charlotte's Hospital", "E45000001", null },
                    { "TBS0739", true, "Petersfield Community Hospital", "E45000019", null },
                    { "TBS0740", true, "Phoenix Day Hospital", "E45000019", null },
                    { "TBS0741", true, "Potters Bar Community Hospital", "E45000017", null },
                    { "TBS0742", true, "Primrose Hill Hospital", "E45000009", null },
                    { "TBS0743", true, "Princess Anne Hospital", "E45000019", null },
                    { "TBS0744", true, "Princess Grace Hospital", "E45000001", null },
                    { "TBS0754", true, "Queen Victoria Hospital [Morecambe]", "E45000018", null },
                    { "TBS0745", true, "Princess Louise Kensington Hospital", "E45000001", null },
                    { "TBS0747", true, "Princess Marina Hospital", "E45000016", null },
                    { "TBS0748", true, "Princess Of Wales Community Hospital", "E45000005", null },
                    { "TBS0749", true, "Princess Royal Hospital [Hull]", "E45000010", null },
                    { "TBS0750", true, "Prospect Park Hospital", "E45000019", null },
                    { "TBS0751", true, "Prudhoe Hospital", "E45000009", null },
                    { "TBS0752", true, "Purley War Memorial Hospital", "E45000001", null },
                    { "TBS0746", true, "Princess Margaret Hospital", "E45000019", null },
                    { "TBS0703", true, "Nottingham Woodthorpe Hospital", "E45000016", null },
                    { "TBS0839", true, "Spire Portsmouth Hospital", "E45000019", null },
                    { "TBS0841", true, "Spire Southampton Hospital", "E45000019", null },
                    { "TBS0932", true, "Wanstead Hospital", "E45000001", null },
                    { "TBS0933", true, "Wantage Community Hospital", "E45000019", null },
                    { "TBS0934", true, "Warley Hospital", "E45000017", null },
                    { "TBS0935", true, "Warminster Community Hospital", "E45000020", null },
                    { "TBS0936", true, "Warninglid Day Hospital", "E45000019", null },
                    { "TBS0937", true, "Waterloo Day Hospital", "E45000018", null },
                    { "TBS0931", true, "Walton Community Hospital", "E45000019", null },
                    { "TBS0938", true, "Wathwood Hospital", "E45000010", null },
                    { "TBS0940", true, "Welland Hospital", "E45000016", null },
                    { "TBS0941", true, "Wellington & District Cottage Hospital", "E45000020", null },
                    { "TBS0942", true, "Wellington Hospital", "E45000001", null },
                    { "TBS0943", true, "Wembley Hospital", "E45000001", null },
                    { "TBS0944", true, "Wesham Park Hospital", "E45000018", null },
                    { "TBS0945", true, "West Berkshire Community Hospital", "E45000019", null },
                    { "TBS0939", true, "Weald Day Hospital", "E45000019", null },
                    { "TBS0946", true, "West Lane Hospital", "E45000009", null },
                    { "TBS0930", true, "Walnut Tree Hospital", "E45000017", null },
                    { "TBS0928", true, "Walkergate Park Hospital", "E45000009", null },
                    { "TBS0914", true, "Totnes Community Hospital", "E45000020", null },
                    { "TBS0915", true, "Towers Hospital", "E45000016", null },
                    { "TBS0916", true, "Trowbridge Community Hospital", "E45000020", null },
                    { "TBS0917", true, "Tyndale Centre Day Hospital", "E45000020", null },
                    { "TBS0918", true, "Tyrell Hospital", "E45000020", null },
                    { "TBS0919", true, "Uckfield Community Hospital", "E45000019", null },
                    { "TBS0929", true, "Wallingford Community Hospital", "E45000019", null },
                    { "TBS0920", true, "Upton Day Hospital [Kent]", "E45000001", null },
                    { "TBS0922", true, "Verrington Hospital", "E45000020", null },
                    { "TBS0923", true, "Victoria Cottage Hospital [Maryport]", "E45000018", null },
                    { "TBS0924", true, "Victoria Hospital [Deal]", "E45000019", null },
                    { "TBS0925", true, "Victoria Hospital [Lichfield]", "E45000005", null },
                    { "TBS0926", true, "Victoria Hospital [Sidmouth]", "E45000020", null },
                    { "TBS0927", true, "Victoria Infirmary [Cheshire]", "E45000018", null },
                    { "TBS0921", true, "Upton House Day Hospital [Northampton]", "E45000016", null },
                    { "TBS0913", true, "Torrington Hospital", "E45000020", null },
                    { "TBS0947", true, "West Mendip Community Hospital", "E45000020", null },
                    { "TBS0949", true, "West Park Hospital [Darlington]", "E45000009", null },
                    { "TBS0967", true, "Willowbank Day Hospital", "E45000020", null },
                    { "TBS0968", true, "Wilson Hospital", "E45000001", null },
                    { "TBS0969", true, "Wimbourne Community Hospital", "E45000020", null },
                    { "TBS0970", true, "Winchcombe Hospital", "E45000020", null },
                    { "TBS0971", true, "Winfield Hospital", "E45000020", null },
                    { "TBS0972", true, "Withernsea Hospital", "E45000010", null },
                    { "TBS0966", true, "Williton Hospital", "E45000020", null },
                    { "TBS0973", true, "Witney Community Hospital", "E45000019", null },
                    { "TBS0975", true, "Wokingham Hospital", "E45000019", null },
                    { "TBS0976", true, "Woodland Hospital [Kettering]", "E45000016", null },
                    { "TBS0977", true, "Woodlands Hospital [Darlington]", "E45000009", null },
                    { "TBS0978", true, "Woods Hospital", "E45000018", null },
                    { "TBS0979", true, "Workington Community Hospital", "E45000018", null },
                    { "TBS0980", true, "Wrexham Chest Clinic", "PHECWAL", null },
                    { "TBS0974", true, "Woking Community Hospital", "E45000019", null },
                    { "TBS0948", true, "West Midlands Hospital", "E45000005", null },
                    { "TBS0965", true, "William Julien Courtauld Hospital", "E45000017", null },
                    { "TBS0964", true, "Wigton Hospital", "E45000018", null },
                    { "TBS0950", true, "Westbury Community Hospital", "E45000020", null },
                    { "TBS0951", true, "Western Eye Hospital", "E45000001", null },
                    { "TBS0952", true, "Westminster Memorial Hospital", "E45000020", null },
                    { "TBS0953", true, "Weston Park Hospital", "E45000010", null },
                    { "TBS0954", true, "Weybridge Community Hospital", "E45000019", null },
                    { "TBS0955", true, "Weymouth Community Hospital", "E45000020", null },
                    { "TBS0999", true, "Willesden Hospital", "E45000001", null },
                    { "TBS0956", true, "Whalley Drive Clinic", "E45000017", null },
                    { "TBS0958", true, "Whelley Hospital", "E45000018", null },
                    { "TBS0959", true, "Whitby Community Hospital", "E45000010", null },
                    { "TBS0960", true, "Whitchurch Hospital [Shropshire]", "E45000005", null },
                    { "TBS0961", true, "White Cross Rehabilitation Hospital", "E45000010", null },
                    { "TBS0962", true, "Whitstable & Tankerton Hospital", "E45000019", null },
                    { "TBS0963", true, "Whitworth Hospital", "E45000016", null },
                    { "TBS0957", true, "Wharfedale General Hospital", "E45000010", null },
                    { "TBS0840", true, "Spire Roding Hospital", "E45000001", null },
                    { "TBS0912", true, "Tonbridge Cottage Hospital", "E45000019", null },
                    { "TBS0910", true, "Tiverton And District Hospital", "E45000020", null },
                    { "TBS0860", true, "St Georges Hospital [Morpeth]", "E45000009", null },
                    { "TBS0861", true, "St Helens Rehabilitation Hospital [York]", "E45000010", null },
                    { "TBS0862", true, "St Johns Hospital", "E45000017", null },
                    { "TBS0863", true, "St Leonards Hospital [Ringwood]", "E45000020", null },
                    { "TBS0864", true, "St Leonards Hospital [Sudbury]", "E45000017", null },
                    { "TBS0865", true, "St Luke's Hospital [Huddersfield]", "E45000010", null },
                    { "TBS0859", true, "St Georges Hospital [Lincoln]", "E45000016", null },
                    { "TBS0866", true, "St Luke's Hospital [Middlesbrough]", "E45000009", null },
                    { "TBS0868", true, "St Marks Hospital [Maidenhead]", "E45000019", null },
                    { "TBS0869", true, "St Martins Hospital [Bath]", "E45000020", null },
                    { "TBS0870", true, "St Martins Hospital [Canterbury]", "E45000019", null },
                    { "TBS0871", true, "St Marys [Gloucester]", "E45000020", null },
                    { "TBS0872", true, "St Mary's Hospital [Leeds]", "E45000010", null },
                    { "TBS0873", true, "St Mary's Hospital [Melton Mowbray]", "E45000016", null },
                    { "TBS0867", true, "St Mark's Hospital [Harrow]", "E45000001", null },
                    { "TBS0874", true, "St Mary's Hospital [Scarborough]", "E45000010", null },
                    { "TBS0858", true, "St Gemma's Hospice", "E45000010", null },
                    { "TBS0856", true, "St Edmunds Hospital [Bury]", "E45000017", null },
                    { "TBS0842", true, "Spire Sussex Hospital", "E45000019", null },
                    { "TBS0843", true, "Spire Thames Valley Hospital", "E45000019", null },
                    { "TBS0844", true, "Spire Washington Hospital", "E45000009", null },
                    { "TBS0845", true, "Springfield Hospital", "E45000017", null },
                    { "TBS0846", true, "St Andrews [Wells]", "E45000020", null },
                    { "TBS0847", true, "St Andrew's Hospital [London]", "E45000001", null },
                    { "TBS0857", true, "St Edmund's Hospital [Northampton]", "E45000016", null },
                    { "TBS0848", true, "St Anne's Hospital [Altrincham]", "E45000018", null },
                    { "TBS0850", true, "St Austell Community Hospital", "E45000020", null },
                    { "TBS0851", true, "St Barnabas Hospital", "E45000020", null },
                    { "TBS0852", true, "St Bartholomews Day Hospital [Liverpool]", "E45000018", null },
                    { "TBS0853", true, "St Catherines Hospital", "E45000018", null },
                    { "TBS0854", true, "St Charles Hospital", "E45000001", null },
                    { "TBS0855", true, "St Christopher's Hospital", "E45000019", null },
                    { "TBS0849", true, "St Ann's Hospital [Poole]", "E45000020", null },
                    { "TBS0911", true, "Tolworth Hospital", "E45000001", null },
                    { "TBS0875", true, "St Mary's Hospital [St Mary's]", "E45000020", null },
                    { "TBS0877", true, "St Michael's Hospital [Hayle]", "E45000020", null },
                    { "TBS0896", true, "Sutton Hospital", "E45000001", null },
                    { "TBS0897", true, "Swanage Community Hospital", "E45000020", null },
                    { "TBS0898", true, "Sylvan Hospital", "E45000019", null },
                    { "TBS0899", true, "Tarporley War Memorial Hospital", "E45000018", null },
                    { "TBS0900", true, "Tavistock Hospital", "E45000020", null },
                    { "TBS0901", true, "Teddington Memorial Hospital", "E45000001", null },
                    { "TBS0895", true, "Surbiton Hospital", "E45000001", null },
                    { "TBS0902", true, "Teignmouth Hospital", "E45000020", null },
                    { "TBS0904", true, "Tewkesbury General Hospital", "E45000020", null },
                    { "TBS0905", true, "Thame Community Hospital", "E45000019", null },
                    { "TBS0906", true, "Thornbury Hospital [Bristol]", "E45000020", null },
                    { "TBS0907", true, "Thornbury Hospital [Sheffield]", "E45000010", null },
                    { "TBS0908", true, "Three Shires Hospital", "E45000016", null },
                    { "TBS0909", true, "Thurrock Community Hospital", "E45000017", null },
                    { "TBS0903", true, "Tenbury Community Hospital", "E45000005", null },
                    { "TBS0876", true, "St Michael's Hospital [Bristol]", "E45000020", null },
                    { "TBS0894", true, "Sunderland Eye Infirmary", "E45000009", null },
                    { "TBS0892", true, "Stroud General Hospital", "E45000020", null },
                    { "TBS0878", true, "St Monicas Hospital", "E45000010", null },
                    { "TBS0879", true, "St Nicholas Hospital", "E45000009", null },
                    { "TBS0880", true, "St Oswalds Hospital", "E45000016", null },
                    { "TBS0881", true, "St Paul's Hospital", "E45000019", null },
                    { "TBS0882", true, "St Peter's Hospital [Maldon]", "E45000017", null },
                    { "TBS0883", true, "St Thomas Hospital [Stockport]", "E45000018", null },
                    { "TBS0893", true, "Stroud Maternity Hospital", "E45000020", null },
                    { "TBS0884", true, "Stamford & Rutland Hospital", "E45000016", null },
                    { "TBS0886", true, "Stead Primary Care Hospital", "E45000009", null },
                    { "TBS0887", true, "Stewart Day Hospital", "E45000018", null },
                    { "TBS0888", true, "Stone House Hospital", "E45000019", null },
                    { "TBS0889", true, "Stonebury Day Hospital", "E45000020", null },
                    { "TBS0890", true, "Stratford Hospital", "E45000005", null },
                    { "TBS0891", true, "Stratton Hospital", "E45000020", null },
                    { "TBS0885", true, "Standish Hospital", "E45000020", null },
                    { "TBS0702", true, "Norwich Community Hospital", "E45000017", null },
                    { "TBS0701", true, "Northgate Hospital [Morpeth]", "E45000009", null },
                    { "TBS0700", true, "Northgate Hospital [Great Yarmouth]", "E45000017", null },
                    { "TBS0514", true, "Cherry Knowle Hospital", "E45000009", null },
                    { "TBS0515", true, "Cherry Tree Hospital", "E45000018", null },
                    { "TBS0516", true, "Cheshunt Community Hospital", "E45000017", null },
                    { "TBS0517", true, "Chingford Hospital", "E45000001", null },
                    { "TBS0518", true, "Chippenham Community Hospital", "E45000020", null },
                    { "TBS0519", true, "Chipping Norton Hospital", "E45000019", null },
                    { "TBS0513", true, "Chelmsford & Essex Hospital", "E45000017", null },
                    { "TBS0520", true, "Cirencester Hospital", "E45000020", null },
                    { "TBS0522", true, "Clevedon Hospital", "E45000020", null },
                    { "TBS0523", true, "Clifton Hospital", "E45000018", null },
                    { "TBS0524", true, "Clitheroe Community Hospital", "E45000018", null },
                    { "TBS0525", true, "Cockermouth Community Hospital", "E45000018", null },
                    { "TBS0984", true, "Congleton War Memorial Hospital", "E45000018", null },
                    { "TBS0526", true, "Cookridge Hospital", "E45000010", null },
                    { "TBS0521", true, "Clayton Hospital", "E45000010", null },
                    { "TBS0527", true, "Coquetdale Cottage Hospital", "E45000009", null },
                    { "TBS0512", true, "Cheadle Royal Hospital", "E45000018", null },
                    { "TBS0510", true, "Chatsworth Suite", "E45000016", null },
                    { "TBS0496", true, "Capio Oaks Hospital", "E45000017", null },
                    { "TBS0497", true, "Capio Reading Hospital", "E45000019", null },
                    { "TBS0498", true, "Capio Rivers Hospital", "E45000017", null },
                    { "TBS0499", true, "Carlton Health Clinic", "E45000016", null },
                    { "TBS0500", true, "Castleberg Hospital", "E45000010", null },
                    { "TBS0501", true, "Castleford & Normanton District Hospital", "E45000010", null },
                    { "TBS0511", true, "Cheadle Hospital- North Staffs Combined Healthcare", "E45000005", null },
                    { "TBS0502", true, "Caterham Dene Hospital", "E45000019", null },
                    { "TBS0504", true, "Chadwell Heath Hospital", "E45000001", null },
                    { "TBS0505", true, "Chalfont's & Gerrards Cross Hospital", "E45000019", null },
                    { "TBS0506", true, "Chantry House Day Hospital", "E45000020", null },
                    { "TBS0507", true, "Chapel Allerton Hospital", "E45000010", null },
                    { "TBS0508", true, "Chard & District Hospital", "E45000020", null },
                    { "TBS0509", true, "Chase Hospital", "E45000019", null },
                    { "TBS0503", true, "Cavendish Hospital", "E45000016", null },
                    { "TBS0495", true, "Calderstones Hospital", "E45000018", null },
                    { "TBS0528", true, "Corby Community Hospital", "E45000016", null },
                    { "TBS0530", true, "Cossham Hospital", "E45000020", null },
                    { "TBS0549", true, "Dove Day Hospital", "E45000019", null },
                    { "TBS0550", true, "Dryden Road Day Hospital", "E45000009", null },
                    { "TBS0551", true, "Duchy Hospital", "E45000010", null },
                    { "TBS0552", true, "Dunedin Hospital", "E45000019", null },
                    { "TBS0553", true, "Durham Community Hospital", "E45000009", null },
                    { "TBS0554", true, "Ech - East Cleveland Hospital", "E45000009", null },
                    { "TBS0548", true, "Dorking General Hospital", "E45000019", null },
                    { "TBS0555", true, "Edenbridge War Memorial Hospital", "E45000019", null },
                    { "TBS0557", true, "Edith Cavell Hospital", "E45000017", null },
                    { "TBS0558", true, "Edward Hain Hospital", "E45000020", null },
                    { "TBS0559", true, "Elderly Day Hospital", "E45000001", null },
                    { "TBS0560", true, "Elizabeth Garrett Anderson Hospital", "E45000001", null },
                    { "TBS0561", true, "Ellen Badger Hospital", "E45000005", null },
                    { "TBS0562", true, "Ellesmere Port Hospital", "E45000018", null },
                    { "TBS0556", true, "Edgbaston Hospital", "E45000005", null },
                    { "TBS0529", true, "Coronation Hospital", "E45000010", null },
                    { "TBS0547", true, "Dilke Memorial Hospital", "E45000020", null },
                    { "TBS0545", true, "Devonshire Road Hospital", "E45000018", null },
                    { "TBS0531", true, "Cranleigh Hospital", "E45000019", null },
                    { "TBS0532", true, "Cranleigh Village Hospital", "E45000019", null },
                    { "TBS0533", true, "Crediton Hospital", "E45000020", null },
                    { "TBS0534", true, "Crewkerne Hospital", "E45000020", null },
                    { "TBS0535", true, "Cromer Hospital", "E45000017", null },
                    { "TBS0536", true, "Cross Lane Hospital", "E45000010", null },
                    { "TBS0546", true, "Didcot Community Hospital", "E45000019", null },
                    { "TBS0537", true, "Crowborough War Memorial Hospital", "E45000019", null },
                    { "TBS0539", true, "Dartmouth Hospital", "E45000020", null },
                    { "TBS0540", true, "Dawlish Hospital", "E45000020", null },
                    { "TBS0541", true, "Dellwood Hospital", "E45000019", null },
                    { "TBS0542", true, "Denmark Road Day Hospital", "E45000020", null },
                    { "TBS0543", true, "Dereham Hospital", "E45000017", null },
                    { "TBS0544", true, "Devizes Community Hospital", "E45000020", null },
                    { "TBS0538", true, "Danetre Hospital", "E45000016", null },
                    { "TBS0563", true, "Elms Day Hospital", "E45000020", null },
                    { "TBS0494", true, "Buxton Hospital", "E45000016", null },
                    { "TBS0492", true, "Burnham On Sea War Memorial Hospital", "E45000020", null },
                    { "TBS0443", true, "Axminster Hospital", "E45000020", null },
                    { "TBS0983", true, "Barking Hospital", "E45000001", null },
                    { "TBS0444", true, "Barrow Hospital", "E45000020", null },
                    { "TBS0445", true, "Barrowby House", "E45000016", null },
                    { "TBS0446", true, "Bath Mineral Hospital", "E45000020", null },
                    { "TBS0447", true, "Bath Road Day Hospital", "E45000019", null },
                    { "TBS0442", true, "Avenue Day Hospital", "E45000018", null },
                    { "TBS0448", true, "Beacon Day Hospital", "E45000018", null },
                    { "TBS0450", true, "Beaumont Hospital", "E45000018", null },
                    { "TBS0451", true, "Beccles & District Hospital", "E45000017", null },
                    { "TBS0452", true, "Beckenham Hospital", "E45000001", null },
                    { "TBS0453", true, "Beighton Community Hospital (The Child & Family Therapy Team)", "E45000010", null },
                    { "TBS0454", true, "Bensham Hospital", "E45000009", null },
                    { "TBS0455", true, "Berkeley Hospital", "E45000020", null },
                    { "TBS0449", true, "Beardwood Hospital", "E45000018", null },
                    { "TBS0456", true, "Bethlem Royal Hospital", "E45000001", null },
                    { "TBS0441", true, "Auckland Park Hospital", "E45000009", null },
                    { "TBS0439", true, "Ashworth Hospital", "E45000018", null },
                    { "TBS0425", true, "Acre Day Hospital", "E45000019", null },
                    { "TBS0426", true, "Adams Day Hospital", "E45000016", null },
                    { "TBS0427", true, "Aintree Hospitals - Opd", "E45000018", null },
                    { "TBS0428", true, "Aldeburgh Hospital", "E45000017", null },
                    { "TBS0429", true, "Alderney Hospital", "E45000020", null },
                    { "TBS0430", true, "Aldrington Day Hospital", "E45000019", null },
                    { "TBS0440", true, "Astley Hospital", "E45000018", null },
                    { "TBS0431", true, "Alfred Bean Hospital", "E45000010", null },
                    { "TBS0433", true, "Amberstone Hospital", "E45000019", null },
                    { "TBS0434", true, "Arundal Hospital Lodge", "E45000019", null },
                    { "TBS0435", true, "Ash Eton", "E45000019", null },
                    { "TBS0436", true, "Ashburton And Buckfastleigh Hospital", "E45000020", null },
                    { "TBS0437", true, "Ashington Hospital", "E45000009", null },
                    { "TBS0438", true, "Ashtead Hospital", "E45000019", null },
                    { "TBS0432", true, "Alton Community Hospital", "E45000019", null },
                    { "TBS0493", true, "Butleigh Hospital", "E45000020", null },
                    { "TBS0457", true, "Bexhill Community Hospital", "E45000019", null },
                    { "TBS0459", true, "Bideford And District Hospital", "E45000020", null },
                    { "TBS0478", true, "Bramcote Hospital", "E45000005", null },
                    { "TBS0479", true, "Brampton War Memorial Hospital", "E45000018", null },
                    { "TBS0480", true, "Brentford Hospital", "E45000001", null },
                    { "TBS0481", true, "Bridgeways Day Hospital", "E45000001", null },
                    { "TBS0482", true, "Bridgwater Hospital", "E45000020", null },
                    { "TBS0483", true, "Bridport Community Hospital", "E45000020", null },
                    { "TBS0477", true, "Bradwell Hospital", "E45000005", null },
                    { "TBS0484", true, "Bristol General Hospital", "E45000020", null },
                    { "TBS0486", true, "Brixham Hospital", "E45000020", null },
                    { "TBS0487", true, "Bromyard Community Hospital", "E45000005", null },
                    { "TBS0488", true, "Brooklands Hospital", "E45000005", null },
                    { "TBS0489", true, "Bucknall Hospital", "E45000005", null },
                    { "TBS0490", true, "Budock Hospital", "E45000020", null },
                    { "TBS0491", true, "Burden Neurological Hospital", "E45000020", null },
                    { "TBS0485", true, "Bristol Homoeopathic Hospital", "E45000020", null },
                    { "TBS0458", true, "Bicester Cottage Hospital", "E45000019", null },
                    { "TBS0476", true, "Brackley Cottage Hospital", "E45000016", null },
                    { "TBS0474", true, "Bovey Tracey Hospital", "E45000020", null },
                    { "TBS0460", true, "Billinge Hospital", "E45000018", null },
                    { "TBS0461", true, "Bingley Hospital", "E45000010", null },
                    { "TBS0462", true, "Birkdale Clinic (Crosby)", "E45000018", null },
                    { "TBS0463", true, "Birmingham Womens Hospital", "E45000005", null }
                });

            migrationBuilder.InsertData(
                table: "TbService",
                columns: new[] { "Code", "IsLegacy", "Name", "PHECCode", "ServiceAdGroup" },
                values: new object[,]
                {
                    { "TBS0464", true, "Bishops Wood Hospital", "E45000001", null },
                    { "TBS0465", true, "Blackberry Hill Hospital", "E45000020", null },
                    { "TBS0475", true, "Bowood Day Hospital", "E45000020", null },
                    { "TBS0466", true, "Blandford Community Hospital", "E45000020", null },
                    { "TBS0468", true, "Bodmin Hospital", "E45000020", null },
                    { "TBS0469", true, "Bolingbroke Hospital", "E45000001", null },
                    { "TBS0470", true, "Bolitho Hospital", "E45000020", null },
                    { "TBS0471", true, "Bootham Park Hospital", "E45000010", null },
                    { "TBS0472", true, "Boscombe Community Hospital", "E45000020", null },
                    { "TBS0473", true, "Bourne Health Clinic", "E45000016", null },
                    { "TBS0467", true, "Blt Private Hospitals", "E45000001", null },
                    { "TBS0564", true, "Emsworth Hospital", "E45000019", null },
                    { "TBS0985", true, "Epsom General Hospital", "E45000019", null },
                    { "TBS0565", true, "Esperance Private Hospital", "E45000019", null },
                    { "TBS0651", true, "Lydney & District Hospital", "E45000020", null },
                    { "TBS0652", true, "Lytham Hospital", "E45000018", null },
                    { "TBS0653", true, "Malham House Day Hospital", "E45000010", null },
                    { "TBS0654", true, "Malmesbury Community Hospital", "E45000020", null },
                    { "TBS0655", true, "Malton Community Hospital", "E45000010", null },
                    { "TBS0656", true, "Malvern Community Hospital", "E45000005", null },
                    { "TBS0650", true, "Lowestoft & North Suffolk Hospital", "E45000017", null },
                    { "TBS0657", true, "Manchester Lifestyle Hospital", "E45000018", null },
                    { "TBS0659", true, "Manor Hospital [Nuneaton]", "E45000005", null },
                    { "TBS0660", true, "Manor Park Hospital", "E45000020", null },
                    { "TBS0661", true, "Mansfield Community Hospital", "E45000016", null },
                    { "TBS0662", true, "Market Drayton Clinic", "E45000005", null },
                    { "TBS0663", true, "Marlow Hospital", "E45000019", null },
                    { "TBS0664", true, "Mary Hewetson Cottage Hospital (Keswick)", "E45000018", null },
                    { "TBS0658", true, "Manor Hospital [Bedford]", "E45000017", null },
                    { "TBS0665", true, "Maudsley Hospital", "E45000001", null },
                    { "TBS0649", true, "Lower Priory Hall Day Hospital", "E45000018", null },
                    { "TBS0647", true, "Longridge Community Hospital", "E45000018", null },
                    { "TBS0635", true, "Lambeth Hospital", "E45000001", null },
                    { "TBS0636", true, "Lancaster Hospital", "E45000018", null },
                    { "TBS0637", true, "Launceston Hospital", "E45000020", null },
                    { "TBS0638", true, "Leatherhead Hospital", "E45000019", null },
                    { "TBS0639", true, "Ledbury Cottage Hospital", "E45000005", null },
                    { "TBS0640", true, "Leicester Frith Hospital", "E45000016", null },
                    { "TBS0648", true, "Longton Hospital", "E45000005", null },
                    { "TBS0641", true, "Lemington Hospital", "E45000009", null },
                    { "TBS0642", true, "Lewes Victoria Hospital", "E45000019", null },
                    { "TBS0991", true, "Lincoln Hospital", "E45000016", null },
                    { "TBS0643", true, "Lings Bar Hospital", "E45000016", null },
                    { "TBS0644", true, "Liskeard Community Hospital", "E45000020", null },
                    { "TBS0645", true, "Little Brook (Apu) Hospital", "E45000019", null },
                    { "TBS0646", true, "Little Court Day Hospital", "E45000020", null },
                    { "TBS0990", true, "Leominster Community Hospital", "E45000005", null },
                    { "TBS0989", true, "Knutsford & District Community Hospital", "E45000018", null },
                    { "TBS0666", true, "Mayfair Day Hospital", "E45000016", null },
                    { "TBS0668", true, "Meadowbank Day Hospital", "E45000016", null },
                    { "TBS0687", true, "Mount Gould Hospital", "E45000020", null },
                    { "TBS0688", true, "Mount Hospital", "E45000019", null },
                    { "TBS0689", true, "Mount Stuart Hospital", "E45000020", null },
                    { "TBS0690", true, "Nevill Hospital", "E45000019", null },
                    { "TBS0691", true, "New Epsom & Ewell Cottage Hospital", "E45000019", null },
                    { "TBS0692", true, "New Hall Hospital", "E45000020", null },
                    { "TBS0686", true, "Mossley Hill Hospital", "E45000018", null },
                    { "TBS0693", true, "New Victoria Hospital", "E45000001", null },
                    { "TBS0695", true, "Newmarket Hospital", "E45000017", null },
                    { "TBS0696", true, "Newquay Hospital", "E45000020", null },
                    { "TBS0697", true, "Newton Abbot Hospital", "E45000020", null },
                    { "TBS0992", true, "Newton Community Hospital", "E45000018", null },
                    { "TBS0698", true, "North Cambridgeshire Hospital", "E45000017", null },
                    { "TBS0699", true, "North Walsham Cottage Hospital", "E45000017", null },
                    { "TBS0694", true, "Newhaven Hillrise Day Hospital", "E45000019", null },
                    { "TBS0667", true, "Mcindoe Surgical Centre", "E45000019", null },
                    { "TBS0685", true, "Morpeth Cottage Hospital", "E45000009", null },
                    { "TBS0683", true, "Moorgreen Hospital", "E45000019", null },
                    { "TBS0669", true, "Melksham Community Hospital", "E45000020", null },
                    { "TBS0670", true, "Middlesex Hospital", "E45000001", null },
                    { "TBS0671", true, "Midhurst Community Hospital", "E45000019", null },
                    { "TBS0672", true, "Milford Hospital", "E45000019", null },
                    { "TBS0673", true, "Milford On Sea War Memorial Hospital", "E45000019", null },
                    { "TBS0674", true, "Mill View Hospital", "E45000019", null },
                    { "TBS0684", true, "Moreton In Marsh Hospital", "E45000020", null },
                    { "TBS0675", true, "Millom Hospital", "E45000018", null },
                    { "TBS0677", true, "Molesey Hospital", "E45000019", null },
                    { "TBS0678", true, "Monkton Hall Hospital", "E45000009", null },
                    { "TBS0679", true, "Monkwearmouth Hospital", "E45000009", null },
                    { "TBS0680", true, "Moore Cottage Hospital", "E45000020", null },
                    { "TBS0681", true, "Moore House", "E45000016", null },
                    { "TBS0682", true, "Moorfields Eye Hospital (City Road)", "E45000001", null },
                    { "TBS0676", true, "Minehead Hospital", "E45000020", null },
                    { "TBS0634", true, "Kington Cottage Hospital", "E45000005", null },
                    { "TBS0633", true, "King Edward Vii Hospital [Midhurst]", "E45000019", null },
                    { "TBS0632", true, "Killingbeck Hospital", "E45000010", null },
                    { "TBS0584", true, "Frome Victoria Hospital", "E45000020", null },
                    { "TBS0585", true, "Fryatt Hospital And Mayflower Medical Centre", "E45000017", null },
                    { "TBS0586", true, "Fulwood Hall Hospital", "E45000018", null },
                    { "TBS0587", true, "Garden Hospital", "E45000001", null },
                    { "TBS0588", true, "General Lying-In Hospital [London]", "E45000001", null },
                    { "TBS0589", true, "Glenside Hospital", "E45000020", null },
                    { "TBS0583", true, "Friary Hospital", "E45000010", null },
                    { "TBS0590", true, "Gloucester House/ Dorian Day Hospital", "E45000020", null },
                    { "TBS0592", true, "Goring Hall Hospital", "E45000019", null },
                    { "TBS0593", true, "Gorse Hill Hospital", "E45000016", null },
                    { "TBS0594", true, "Goscote Hospital", "E45000005", null },
                    { "TBS0595", true, "Gosport War Memorial Hospital", "E45000019", null },
                    { "TBS0986", true, "Greater Manchester West Mental Health Nhs Trust", "E45000018", null },
                    { "TBS0596", true, "Grove Convalescent Hospital", "E45000010", null },
                    { "TBS0591", true, "Gordon Hospital", "E45000001", null },
                    { "TBS0597", true, "Grove Hospital", "E45000016", null },
                    { "TBS0582", true, "Franklyn Community Hospital", "E45000020", null },
                    { "TBS0580", true, "Fordingbridge Hospital", "E45000019", null },
                    { "TBS0566", true, "Euxton Hall Hospital", "E45000018", null },
                    { "TBS0567", true, "Evesham Community Hospital", "E45000005", null },
                    { "TBS0568", true, "Exmouth Hospital", "E45000020", null },
                    { "TBS0569", true, "Fairford Hospital", "E45000020", null },
                    { "TBS0570", true, "Falmouth Hospital", "E45000020", null },
                    { "TBS0571", true, "Farnham Hospital", "E45000019", null },
                    { "TBS0581", true, "Foscote Hospital", "E45000019", null },
                    { "TBS0572", true, "Farnham Road Hospital", "E45000019", null },
                    { "TBS0574", true, "Felixstowe Hospital", "E45000017", null },
                    { "TBS0575", true, "Fenwick Hospital (Peripheral Clinic)", "E45000019", null },
                    { "TBS0576", true, "Fieldhead Hospital", "E45000010", null },
                    { "TBS0577", true, "Finchley Memorial Hospital", "E45000001", null },
                    { "TBS0578", true, "Fleet Hospital", "E45000019", null },
                    { "TBS0579", true, "Fleetwood Hospital", "E45000018", null },
                    { "TBS0573", true, "Faversham Cottage Hospital", "E45000019", null },
                    { "TBS0598", true, "Gulson Hospital", "E45000005", null },
                    { "TBS0599", true, "Halstead Hospital", "E45000017", null },
                    { "TBS0600", true, "Haltwhistle War Memorial Hospital", "E45000009", null },
                    { "TBS0620", true, "Holmevalley Memorial Hospital", "E45000010", null },
                    { "TBS0621", true, "Holsworthy Hospital", "E45000020", null },
                    { "TBS0622", true, "Homelands Hospital", "E45000009", null },
                    { "TBS0623", true, "Homeopathic Hospital", "E45000019", null },
                    { "TBS0624", true, "Horn Hall Hospital", "E45000009", null },
                    { "TBS0625", true, "Hornsea Cottage Hospital", "E45000010", null },
                    { "TBS0619", true, "Hollins Park Hospital Geriatric Daycare", "E45000018", null },
                    { "TBS0626", true, "Hrh Princess Christian's Hospital", "E45000019", null },
                    { "TBS0628", true, "Hunters Moor Hospital", "E45000009", null },
                    { "TBS0629", true, "Hyde Hospital", "E45000018", null },
                    { "TBS0630", true, "Hythe Hospital (Peripheral Clinic)", "E45000019", null },
                    { "TBS0987", true, "John Coupland Hospital", "E45000016", null },
                    { "TBS0988", true, "Johnson Hospital", "E45000016", null },
                    { "TBS0631", true, "Keynsham Hospital", "E45000020", null },
                    { "TBS0627", true, "Hsh Broadmoor Hospital", "E45000019", null },
                    { "TBS0618", true, "Holbeach Hospital", "E45000016", null },
                    { "TBS0617", true, "Highwood Hospital", "E45000017", null },
                    { "TBS0616", true, "Highbury Hospital", "E45000016", null },
                    { "TBS0601", true, "Ham Green Hospital", "E45000020", null },
                    { "TBS0602", true, "Harbour Hospital", "E45000020", null },
                    { "TBS0603", true, "Harpenden Memorial Hospital", "E45000017", null },
                    { "TBS0604", true, "Harperbury Hospital", "E45000017", null },
                    { "TBS0605", true, "Harplands Hospital", "E45000005", null },
                    { "TBS0606", true, "Hartismere Hospital", "E45000017", null },
                    { "TBS0607", true, "Haslemere Hospital", "E45000019", null },
                    { "TBS0608", true, "Havant War Memorial Hospital", "E45000019", null },
                    { "TBS0609", true, "Hawkhurst Hospital", "E45000019", null },
                    { "TBS0610", true, "Haywood Hospital", "E45000005", null },
                    { "TBS0611", true, "Heavitree Hospital", "E45000020", null },
                    { "TBS0612", true, "Helston Hospital", "E45000020", null },
                    { "TBS0613", true, "Herbert Hospital", "E45000020", null },
                    { "TBS0614", true, "Hertford County Hospital", "E45000017", null },
                    { "TBS0615", true, "Herts And Essex Hospital", "E45000017", null },
                    { "TBS0981", true, "Yeatman Hospital", "E45000020", null },
                    { "TBS0982", true, "Zachary Merton Hospital", "E45000019", null }
                });

            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "IsLegacy", "Name", "TBServiceCode" },
                values: new object[,]
                {
                    { new Guid("1200824a-cce6-491d-bd93-33e44f0b383b"), "E92000001", true, "ACCRINGTON VICTORIA HOSPITAL", "TBS0424" },
                    { new Guid("13958bea-c1bc-4ccf-ad03-e9bbfa016502"), "E92000001", true, "RUSHDEN MEMORIAL CLINIC", "TBS0790" },
                    { new Guid("29d4ded5-bcfb-4cfd-b6da-1f3ef98d3a64"), "E92000001", true, "RUTH LANCASTER JAMES HOSPITAL (ALSTON MATERNITY)", "TBS0791" },
                    { new Guid("f128c767-8664-40b9-a086-fd929ae133a2"), "E92000001", true, "RUTSON HOSPITAL", "TBS0792" },
                    { new Guid("af29e41e-2f82-41ac-983e-ff1df2294e58"), "E92000001", true, "RYE MEMORIAL HOSPITAL", "TBS0793" },
                    { new Guid("d8705a5e-3662-406d-a540-afa447aabca3"), "E92000001", true, "RYHOPE GENERAL HOSPITAL", "TBS0794" },
                    { new Guid("becd2e23-8d27-42b8-9b03-d96571023580"), "E92000001", true, "SAFFRON WALDEN COMMUNITY HOSPITAL", "TBS0795" },
                    { new Guid("25bf770e-faa6-4e5e-b412-a125ddec94ed"), "E92000001", true, "RUNWELL HOSPITAL", "TBS0789" },
                    { new Guid("64150436-2aac-45af-b6c0-5f948dc3cbf3"), "E92000001", true, "SAMARITAN HOSPITAL FOR WOMEN", "TBS0796" },
                    { new Guid("2a6fa966-d70f-48f0-a72a-782827ee1974"), "E92000001", true, "SARUM ROAD HOSPITAL", "TBS0798" },
                    { new Guid("d43a3585-c851-48ae-9ffd-8cf9045befc2"), "E92000001", true, "SAVERNAKE HOSPITAL", "TBS0799" },
                    { new Guid("e798fcd0-c648-4a84-aa52-4274711438e1"), "E92000001", true, "SAXON CLINIC", "TBS0800" },
                    { new Guid("ead4dc97-a84a-4deb-a1ce-73cf295a0095"), "E92000001", true, "SCALEBOR PARK HOSPITAL", "TBS0801" },
                    { new Guid("3fe6c7bf-c0bd-4b9f-87f5-1bac12969a91"), "E92000001", true, "SCARSDALE HOSPITAL", "TBS0802" },
                    { new Guid("f376d84f-37b1-4013-bb85-209788983603"), "E92000001", true, "SCOTT HOSPITAL", "TBS0803" },
                    { new Guid("e9241b59-41a5-463a-8933-2abe381d91b6"), "E92000001", true, "SANDRINGHAM HOSPITAL", "TBS0797" },
                    { new Guid("4a8aa3dc-1da9-42cd-b9ae-4e882255d86a"), "E92000001", true, "SEAFORD DAY HOSPITAL", "TBS0804" },
                    { new Guid("768011c9-e0c4-4b1b-84a3-317c4bf2f402"), "E92000001", true, "ROYSTON HOSPITAL", "TBS0788" },
                    { new Guid("c109b0b2-33f3-4d29-8d37-e25623d9a2f6"), "E92000001", true, "ROYAL NATIONAL ORTHOPAEDIC HOSPITAL (BOLSOVER STREET)", "TBS0786" },
                    { new Guid("abe80c99-0bbf-4e48-933b-874b65358eca"), "E92000001", true, "ROSS COMMUNITY HOSPITAL", "TBS0775" },
                    { new Guid("52399c6d-9d4a-4286-963f-7ec91adf946e"), "E92000001", true, "ROSSENDALE HOSPITAL", "TBS0994" },
                    { new Guid("ba9b7f24-7e91-42ca-abfe-ce530f26aa36"), "E92000001", true, "ROTHBURY COMMUNITY HOSPITAL", "TBS0776" },
                    { new Guid("6a4c0d9b-fcbe-44b5-900f-558b10ed9ecc"), "E92000001", true, "ROUNDWAY HOSPITAL", "TBS0777" },
                    { new Guid("d7d5c158-2e6f-4133-a601-14d729efb1f5"), "E92000001", true, "ROWLEY HOSPITAL", "TBS0778" },
                    { new Guid("b95f2b70-0a48-4470-bf16-8ba6fc1a2264"), "E92000001", true, "ROWLEY REGIS HOSPITAL", "TBS0779" },
                    { new Guid("11b304c6-f0bc-498f-925b-1fc8e1c7031a"), "E92000001", true, "ROYAL NATIONAL THROAT, NOSE & EAR HOSPITAL", "TBS0787" },
                    { new Guid("459820f1-9c30-4998-a207-9dbada17108c"), "E92000001", true, "ROXBOURNE HOSPITAL", "TBS0780" },
                    { new Guid("18b6e8b8-c327-4f37-9143-c215bad7faf3"), "E92000001", true, "ROYAL EYE INFIRMARY", "TBS0781" },
                    { new Guid("64a797c5-6852-4095-97ae-3bd45beb357f"), "S92000003", true, "ROYAL INFIRMARY OF EDINBURGH", "TBS0782" },
                    { new Guid("c1a0da74-9e02-43fc-beec-14589057478c"), "E92000001", true, "ROYAL LEAMINGTON SPA REHABILITATION HOSPITAL", "TBS0783" },
                    { new Guid("323f047e-5341-424a-886c-0324ab669fe7"), "E92000001", true, "ROYAL LONDON HOMEOPATHIC HOSPITAL", "TBS0784" },
                    { new Guid("a9820956-84cd-4373-8e93-04eb01c45c87"), "E92000001", true, "ROYAL MARSDEN HOSPITAL (SURREY)", "TBS0996" },
                    { new Guid("79236fd5-e97d-45e7-9a5b-5ee9b623c6d3"), "E92000001", true, "ROYAL NATIONAL HOSPITAL FOR RHEUMATIC DISEASES", "TBS0785" },
                    { new Guid("6a4403f4-4e9d-44d5-b78e-b330547bb781"), "E92000001", true, "ROYAL ALEXANDRA CHILDREN'S HOSPITAL [BRIGHTON]", "TBS0995" },
                    { new Guid("39ef6af3-6c00-4d52-83fe-39f398809e46"), "E92000001", true, "ROSIE HOSPITAL", "TBS0774" },
                    { new Guid("d27fe1ae-3758-48db-9da9-7c0ea3c6c5ca"), "E92000001", true, "SELBY & DISTRICT WAR MEMORIAL HOSPITAL", "TBS0805" },
                    { new Guid("d20f8b13-6717-47ca-a235-f772a8d16514"), "E92000001", true, "SHELBURNE HOSPITAL", "TBS0807" },
                    { new Guid("e251a3ab-ebcf-4128-a93d-aefb37b39fda"), "E92000001", true, "SPIRE BRISTOL HEALTH CLINIC", "TBS0824" },
                    { new Guid("5c5cbc05-7e6a-4b10-add0-7f7638ec1918"), "E92000001", true, "SPIRE BUSHEY HOSPITAL", "TBS0825" },
                    { new Guid("266014b2-0bc5-4daf-9bff-219f1c77db68"), "E92000001", true, "SPIRE CHESHIRE HOSPITAL", "TBS0826" },
                    { new Guid("fcaec936-3aff-4b19-a1a0-048ea2a06ae0"), "E92000001", true, "SPIRE CLARE PARK HOSPITAL", "TBS0827" },
                    { new Guid("6a7f7bd2-5667-4cf8-a857-e8ae47871898"), "E92000001", true, "SPIRE DUNEDIN HOSPITAL", "TBS0828" },
                    { new Guid("34a57071-c138-4f11-b5e8-15bbc4b5462d"), "E92000001", true, "SPIRE ELLAND HOSPITAL", "TBS0829" },
                    { new Guid("b50b66ed-8182-4ef8-9b7d-848f14cb3e91"), "E92000001", true, "SPIRE BARNSLEY CONSULTING ROOMS", "TBS0823" },
                    { new Guid("26b15340-f812-4993-87d8-1a6e32c8115c"), "E92000001", true, "SPIRE FYLDE COAST HOSPITAL", "TBS0830" },
                    { new Guid("d0602bd5-530d-474e-bf74-532bfbfeb6d8"), "E92000001", true, "SPIRE HULL AND EAST RIDING HOSPITAL", "TBS0832" },
                    { new Guid("053ae50b-3b39-44b7-89ec-1bce415bd846"), "E92000001", true, "SPIRE LEEDS HOSPITAL", "TBS0833" },
                    { new Guid("8fa5e547-b7c7-45cb-8912-8609d6c16f1d"), "E92000001", true, "SPIRE LEICESTER HOSPITAL", "TBS0834" },
                    { new Guid("6e03ad18-2623-42cf-a39a-1a78dcef8714"), "E92000001", true, "SPIRE LITTLE ASTON HOSPITAL", "TBS0835" },
                    { new Guid("6db4a299-9431-4e71-a0d9-c97b186ce043"), "S92000003", true, "SPIRE LIVINGSTON CLINIC", "TBS0836" },
                    { new Guid("48b7a7b0-ed79-45af-92cf-25445c376102"), "E92000001", true, "SPIRE METHLEY PARK HOSPITAL", "TBS0837" },
                    { new Guid("f81364f2-4002-4d20-8728-3997c24a62b4"), "E92000001", true, "SPIRE GATWICK PARK HOSPITAL", "TBS0831" },
                    { new Guid("37187ac2-5da3-46a1-b6b1-2dff7c8c9720"), "E92000001", true, "SEVENOAKS HOSPITAL", "TBS0806" },
                    { new Guid("733fd6d6-be41-42ed-b105-68acd32b678f"), "E92000001", true, "SOUTHWOLD HOSPITAL", "TBS0822" },
                    { new Guid("84576348-2dca-4487-b0a9-46268f222fa6"), "E92000001", true, "SOUTHLANDS HOSPITAL", "TBS0998" },
                    { new Guid("5d3afb26-c8a6-44f6-a00a-f50d23fb220e"), "E92000001", true, "SHEPPEY COMMUNITY HOSPITAL", "TBS0808" },
                    { new Guid("f1484bb2-5277-45f9-8549-0c4ea1255aff"), "E92000001", true, "SHEPTON MALLET COMMUNITY HOSPITAL", "TBS0809" },
                    { new Guid("67956308-b9df-4107-b50f-3f6c911005ae"), "E92000001", true, "SHIPLEY HOSPITAL", "TBS0810" },
                    { new Guid("6f637c51-b8aa-4fff-a796-cf69bdaa49f7"), "E92000001", true, "SHIREHILL HOSPITAL", "TBS0811" },
                    { new Guid("910d3ae1-0c2c-4ed6-baac-0e1e69260969"), "E92000001", true, "SHIRLEY OAKS HOSPITAL", "TBS0812" },
                    { new Guid("4485e161-a44c-43fd-b43f-a038f3beb012"), "E92000001", true, "SIR ALFRED JONES MEMORIAL HOSPITAL (ACUTE)", "TBS0813" },
                    { new Guid("6344cd67-0f25-4288-9026-6af917df5790"), "E92000001", true, "SOUTHPORT GENERAL INFIRMARY", "TBS0821" },
                    { new Guid("485bf2da-6079-4160-bd84-8eb30559c63c"), "E92000001", true, "SITTINGBOURNE MEMORIAL HOSPITAL", "TBS0814" },
                    { new Guid("31b6677d-f5df-4101-950d-cd47878edf55"), "E92000001", true, "SKIPTON GENERAL HOSPITAL", "TBS0815" },
                    { new Guid("a22dda9e-0c7d-475f-ba10-6b76d549f920"), "E92000001", true, "SOUTH HAMS HOSPITAL", "TBS0816" },
                    { new Guid("4970ea1f-b07d-4290-98c7-1cbfc055829e"), "E92000001", true, "SOUTH MOLTON HOSPITAL", "TBS0817" },
                    { new Guid("5b41832a-3656-4a4c-b3a7-e6d3d67d76d8"), "E92000001", true, "SOUTH MOOR HOSPITAL", "TBS0818" },
                    { new Guid("f13b200d-fd21-4078-a951-fc5fc331718d"), "E92000001", true, "SOUTH PETHERTON HOSPITAL", "TBS0819" },
                    { new Guid("90cd195c-ca0b-41fe-a04b-70d2c90c15b7"), "E92000001", true, "SOUTH SHORE HOSPITAL", "TBS0820" },
                    { new Guid("ac84273e-bbe7-4720-b731-5892e4ee1b2c"), "E92000001", true, "SKEGNESS & DISTRICT GENERAL HOSPITAL", "TBS0997" },
                    { new Guid("c72ae942-1baa-4136-a21f-39438bf52f71"), "E92000001", true, "SPIRE NORWICH HOSPITAL", "TBS0838" },
                    { new Guid("fd8337ea-cb3f-46b0-8586-fdd9dbd64862"), "E92000001", true, "ROMSEY HOSPITAL", "TBS0773" },
                    { new Guid("171806a0-98a7-4ae6-bac2-ffcb4f3fc092"), "E92000001", true, "ROCHFORD COMMUNITY HOSPITAL", "TBS0771" },
                    { new Guid("f688b1b4-7f21-42ef-b23e-9d69ea97cd05"), "E92000001", true, "OXFORD CLINIC", "TBS0722" },
                    { new Guid("aeb507d7-b0fc-466c-a26a-c2b4165bd01a"), "E92000001", true, "OXTED & LIMPSFIELD HOSPITAL", "TBS0723" },
                    { new Guid("d21489d6-7d1e-46fb-9d11-c4b536285347"), "E92000001", true, "PADDOCKS CLINIC", "TBS0724" },
                    { new Guid("69bddd08-ed80-4885-869c-4714dc02ce06"), "E92000001", true, "PAIGNTON HOSPITAL", "TBS0725" },
                    { new Guid("ae74328a-9f61-4d1e-9247-4895ae7045da"), "E92000001", true, "PALMER COMMUNITY HOSPITAL", "TBS0726" },
                    { new Guid("5524785c-7823-41df-9341-6b4a65aa9c19"), "E92000001", true, "PARK HILL HOSPITAL", "TBS0727" },
                    { new Guid("8a702ecd-96fd-457f-a931-4b367f12277a"), "E92000001", true, "OTTERY ST MARY HOSPITAL", "TBS0721" },
                    { new Guid("8a7d861d-5086-4fba-a813-20fb895c8412"), "E92000001", true, "PARK HOSPITAL [NOTTINGHAM]", "TBS0728" },
                    { new Guid("9670dd65-0d74-423c-ad51-cd1d4c638d03"), "E92000001", true, "PARK LANE MEDICAL CENTRE", "TBS0730" },
                    { new Guid("4340fe30-9786-4feb-9d90-e1c82f340113"), "E92000001", true, "PARK VIEW CLINIC", "TBS0731" },
                    { new Guid("14f099af-044d-4a30-ae8d-6aef04dbd1a1"), "E92000001", true, "PARK VIEW DAY HOSPITAL", "TBS0732" },
                    { new Guid("89a79fd3-e59f-4255-9e7a-2b536dbb5075"), "E92000001", true, "PARKLANDS HOSPITAL", "TBS0733" },
                    { new Guid("4fcd15ef-14c7-454b-aae4-4ecdb49f9d6e"), "E92000001", true, "PARKWOOD HOSPITAL", "TBS0734" },
                    { new Guid("f5326f7f-d87f-4f89-b7c2-36220872fcaf"), "E92000001", true, "PATRICK STEAD HOSPITAL", "TBS0735" },
                    { new Guid("8e2e428a-341c-4f76-9633-124d5ca342b5"), "E92000001", true, "PARK HOSPITAL [OXFORD]", "TBS0729" },
                    { new Guid("39530369-9873-483e-a218-114c3bcc6240"), "E92000001", true, "PAULTON MEMORIAL HOSPITAL", "TBS0736" },
                    { new Guid("ac1f4e4a-6f25-4349-b7a3-8966bd6ac2db"), "E92000001", true, "OLD COTTAGE HOSPITAL", "TBS0720" },
                    { new Guid("7c6e024a-27a7-4763-b1a7-30f442aea7bf"), "E92000001", true, "NUFFIELD HEALTH WOLVERHAMPTON HOSPITAL", "TBS0718" },
                    { new Guid("b1b3cdb9-8e54-4e1b-b94c-9cbe63d3d86a"), "E92000001", true, "NUFFIELD HEALTH BRIGHTON HOSPITAL", "TBS0704" },
                    { new Guid("b6d8ea7c-1d31-4102-baf2-a799480ca577"), "E92000001", true, "NUFFIELD HEALTH BRISTOL HOSPITAL", "TBS0705" },
                    { new Guid("391ed44b-76fb-4cd1-8eb8-00ad07336412"), "E92000001", true, "NUFFIELD HEALTH CHELTENHAM HOSPITAL", "TBS0706" },
                    { new Guid("41230103-eafc-4e94-8a18-00a4233ae90d"), "E92000001", true, "NUFFIELD HEALTH EXETER HOSPITAL", "TBS0707" },
                    { new Guid("1e2e25df-bd19-4647-82f0-bd3b3af5ec19"), "E92000001", true, "NUFFIELD HEALTH HAMPSHIRE HOSPITAL", "TBS0708" },
                    { new Guid("c88a545c-fe4a-4bff-ab44-0f00f40e721f"), "E92000001", true, "NUFFIELD HEALTH IPSWICH HOSPITAL", "TBS0709" },
                    { new Guid("11081272-a36b-459d-9e6b-7e8c486fdc9f"), "E92000001", true, "OKEHAMPTON COMMUNITY HOSPITAL", "TBS0719" },
                    { new Guid("9de16b29-e316-4d88-ad2a-e47a0b413a3d"), "E92000001", true, "NUFFIELD HEALTH LEICESTER HOSPITAL", "TBS0710" },
                    { new Guid("0dcae460-f8cc-44af-b6c1-8b86b0a725d5"), "E92000001", true, "NUFFIELD HEALTH NORTH STAFFORDSHIRE HOSPITAL", "TBS0712" },
                    { new Guid("a4eaac12-239b-4e65-ac79-e7858080a94e"), "E92000001", true, "NUFFIELD HEALTH PLYMOUTH HOSPITAL", "TBS0713" },
                    { new Guid("aa778a08-5ad9-4268-b8c0-f457c8b591ab"), "E92000001", true, "NUFFIELD HEALTH TAUNTON HOSPITAL", "TBS0714" },
                    { new Guid("f99825d4-8949-42bd-af1c-3ad4eb9b14e1"), "E92000001", true, "NUFFIELD HEALTH TEES HOSPITAL", "TBS0715" },
                    { new Guid("2c222cd0-880d-4b83-9e61-31b820c01626"), "E92000001", true, "NUFFIELD HEALTH THE GROSVENOR HOSPITAL CHESTER", "TBS0716" },
                    { new Guid("06749b63-ae06-4cc6-9d66-9dd5c4a0245e"), "E92000001", true, "NUFFIELD HEALTH THE MANOR HOSPITAL OXFORD", "TBS0717" },
                    { new Guid("c267a057-4558-4f6a-aa42-3a034e868d64"), "E92000001", true, "NUFFIELD HEALTH NEWCASTLE-UPON-TYNE HOSPITAL", "TBS0711" },
                    { new Guid("af1a9b2b-5a89-4b4b-a434-d58e123afc32"), "E92000001", true, "ROCKWELL DAY HOSPITAL", "TBS0772" },
                    { new Guid("70693b46-61f4-4321-a896-a9913d0aa600"), "E92000001", true, "PEMBERTON CLINIC", "TBS0993" },
                    { new Guid("7306f844-2521-48d0-a765-2c1e45621d88"), "E92000001", true, "PETERLEE COMMUNITY HOSPITAL", "TBS0738" },
                    { new Guid("179b430d-5d8c-4f21-9ba8-c6a652df0d62"), "E92000001", true, "RAMPTON HOSPITAL", "TBS0757" },
                    { new Guid("acf1a783-686a-4ef2-a3ca-0e87a7006541"), "E92000001", true, "RAMSBOTTOM COTTAGE HOSPITAL", "TBS0758" },
                    { new Guid("d907b8c5-ea96-4ded-a696-5e906b2537bc"), "E92000001", true, "RATHBONE HOSPITAL", "TBS0759" },
                    { new Guid("7c5a090e-b20b-4dd0-938d-03aabc914eb2"), "E92000001", true, "RAVENSCOURT PARK HOSPITAL", "TBS0760" },
                    { new Guid("a4da4b5d-3907-4962-88c7-04b387471131"), "E92000001", true, "REDCLIFFE DAY HOSPITAL", "TBS0761" },
                    { new Guid("cebbe2c3-f6e8-49a9-adfc-7264fe6f0539"), "E92000001", true, "RENACRES HOSPITAL", "TBS0762" },
                    { new Guid("2b0c8ab0-4909-4c9c-9441-fc510b936d41"), "E92000001", true, "RADFORD HEALTH CENTRE", "TBS0756" },
                    { new Guid("88b3881c-9401-480c-8f95-53fa0ad345c2"), "E92000001", true, "RIBBLETON HOSPITAL", "TBS0763" },
                    { new Guid("31c14107-8e42-43d6-8dfe-bbe5b19f1d4a"), "E92000001", true, "RIDGE LEA HOSPITAL", "TBS0765" },
                    { new Guid("251ca4b4-7699-4927-a36c-f2d53f737d1f"), "E92000001", true, "RIDLEY DAY HOSPITAL", "TBS0766" },
                    { new Guid("bd77d4f6-5a7e-48e4-a4fb-6c2e6bb41a60"), "E92000001", true, "RIPON AND DISTRICT COMMUNITY HOSPITAL", "TBS0767" },
                    { new Guid("2082a678-403f-4969-8b07-557ce2240409"), "E92000001", true, "RIVERS HOSPITAL", "TBS0768" },
                    { new Guid("805559be-9862-4194-9a37-9a9d93b73be6"), "E92000001", true, "ROBERT JONES & AGNES HUNT", "TBS0769" },
                    { new Guid("8b3b8368-fcf4-4612-b78a-e7a00e7fa826"), "E92000001", true, "ROBOROUGH DAY HOSPITAL", "TBS0770" },
                    { new Guid("57e673cc-ed70-442f-bd96-0f261fc88c8c"), "E92000001", true, "RICHARDSON HOSPITAL", "TBS0764" },
                    { new Guid("d2558cd1-71ca-4c7f-802d-98498b2eaf90"), "E92000001", true, "PENRITH HOSPITAL", "TBS0737" },
                    { new Guid("48fc350e-6fd5-4087-9b3d-a5b2c43c2b45"), "E92000001", true, "QUEEN VICTORIA MEMORIAL HOSPITAL", "TBS0755" },
                    { new Guid("b1ce8793-4601-44ff-b71b-fdd3ccec7cfd"), "E92000001", true, "QUEEN CHARLOTTE'S HOSPITAL", "TBS0753" },
                    { new Guid("4de9449e-65f5-40c0-9c32-953d189f20df"), "E92000001", true, "PETERSFIELD COMMUNITY HOSPITAL", "TBS0739" },
                    { new Guid("a6ee57cb-dda6-4d29-852c-62b762d147a1"), "E92000001", true, "PHOENIX DAY HOSPITAL", "TBS0740" },
                    { new Guid("12759488-b4db-4f7c-ab2e-f13bd8c6e73d"), "E92000001", true, "POTTERS BAR COMMUNITY HOSPITAL", "TBS0741" },
                    { new Guid("4836a4e5-d0a3-43de-b8e7-d4bfe81b7e9c"), "E92000001", true, "PRIMROSE HILL HOSPITAL", "TBS0742" },
                    { new Guid("16aba4bf-274c-465e-9bfd-ff3309266e97"), "E92000001", true, "PRINCESS ANNE HOSPITAL", "TBS0743" },
                    { new Guid("5740e112-41b7-4b5a-aec0-1d15fdb6d997"), "E92000001", true, "PRINCESS GRACE HOSPITAL", "TBS0744" },
                    { new Guid("96ec72d1-fdcf-4988-b73a-e3fc5a83f49d"), "E92000001", true, "QUEEN VICTORIA HOSPITAL [MORECAMBE]", "TBS0754" },
                    { new Guid("0a77a26e-c6cf-4f2d-8501-8d94ea136d1d"), "E92000001", true, "PRINCESS LOUISE KENSINGTON HOSPITAL", "TBS0745" },
                    { new Guid("0c144e14-ed87-4980-b962-6480bb945afd"), "E92000001", true, "PRINCESS MARINA HOSPITAL", "TBS0747" },
                    { new Guid("f11294d0-1f00-4582-9d24-3707bf5280d6"), "E92000001", true, "PRINCESS OF WALES COMMUNITY HOSPITAL", "TBS0748" },
                    { new Guid("f3083498-736f-421a-a001-feb4cf951422"), "E92000001", true, "PRINCESS ROYAL HOSPITAL [HULL]", "TBS0749" },
                    { new Guid("aa2806f9-ea36-48b1-a5b2-04fee3f81a0d"), "E92000001", true, "PROSPECT PARK HOSPITAL", "TBS0750" },
                    { new Guid("bc5a7dc6-be8a-47f3-b02e-5b2efe6a379e"), "E92000001", true, "PRUDHOE HOSPITAL", "TBS0751" },
                    { new Guid("2b73ae84-1c40-4098-9679-86f512999682"), "E92000001", true, "PURLEY WAR MEMORIAL HOSPITAL", "TBS0752" },
                    { new Guid("9a6d0dfa-73f2-4df5-be61-95e4834cf564"), "E92000001", true, "PRINCESS MARGARET HOSPITAL", "TBS0746" },
                    { new Guid("27988285-43f2-41d6-a40b-a98fe588da6e"), "E92000001", true, "NOTTINGHAM WOODTHORPE HOSPITAL", "TBS0703" },
                    { new Guid("d383e248-a391-4cad-8394-85871b372412"), "E92000001", true, "SPIRE PORTSMOUTH HOSPITAL", "TBS0839" },
                    { new Guid("671be757-50e5-4636-912e-707241af5149"), "E92000001", true, "SPIRE SOUTHAMPTON HOSPITAL", "TBS0841" },
                    { new Guid("335fe746-12f2-4c42-9f28-7f21a3535ad4"), "E92000001", true, "WANSTEAD HOSPITAL", "TBS0932" },
                    { new Guid("61ed6301-8b4a-4ed0-93ba-2fa07b5c8796"), "E92000001", true, "WANTAGE COMMUNITY HOSPITAL", "TBS0933" },
                    { new Guid("a6f34849-54f7-44e7-8fce-265a9a8b604f"), "E92000001", true, "WARLEY HOSPITAL", "TBS0934" },
                    { new Guid("57725cc0-5c4d-4ac9-8de6-5355cd8abe66"), "E92000001", true, "WARMINSTER COMMUNITY HOSPITAL", "TBS0935" },
                    { new Guid("d5b0e5e7-051f-4f72-9980-288125b680f1"), "E92000001", true, "WARNINGLID DAY HOSPITAL", "TBS0936" },
                    { new Guid("91c4fcb5-6ca9-468f-9085-55d267414263"), "E92000001", true, "WATERLOO DAY HOSPITAL", "TBS0937" },
                    { new Guid("1219b1c8-850c-483f-bf3b-16dc9090dc7b"), "E92000001", true, "WALTON COMMUNITY HOSPITAL", "TBS0931" },
                    { new Guid("93ccb810-7dfd-44a5-afe4-69c9d9c6d314"), "E92000001", true, "WATHWOOD HOSPITAL", "TBS0938" },
                    { new Guid("7e1071f0-410c-4f32-8dd0-3763d23605e3"), "E92000001", true, "WELLAND HOSPITAL", "TBS0940" },
                    { new Guid("80018752-3ab1-4285-b26f-34655f5c4ed7"), "E92000001", true, "WELLINGTON & DISTRICT COTTAGE HOSPITAL", "TBS0941" },
                    { new Guid("ecad63ff-94da-48ff-927a-db56bff7b4e7"), "E92000001", true, "WELLINGTON HOSPITAL", "TBS0942" },
                    { new Guid("a53921b3-05d5-485c-9dcf-9cd7371d5a33"), "E92000001", true, "WEMBLEY HOSPITAL", "TBS0943" },
                    { new Guid("042fa0e4-791a-47a0-9f5f-2c1348954954"), "E92000001", true, "WESHAM PARK HOSPITAL", "TBS0944" },
                    { new Guid("60e405a2-4d47-4a49-a739-dbedfb7bf9b3"), "E92000001", true, "WEST BERKSHIRE COMMUNITY HOSPITAL", "TBS0945" },
                    { new Guid("eeaeea6e-08c1-4096-9866-588752a91764"), "E92000001", true, "WEALD DAY HOSPITAL", "TBS0939" },
                    { new Guid("f8eeccf0-529d-4a86-ac0a-8795a315b33a"), "E92000001", true, "WEST LANE HOSPITAL", "TBS0946" },
                    { new Guid("e5afd154-c762-434b-ac16-a54d71737d31"), "E92000001", true, "WALNUT TREE HOSPITAL", "TBS0930" },
                    { new Guid("22e73828-676d-4c3e-bc25-092cb00c0ae0"), "E92000001", true, "WALKERGATE PARK HOSPITAL", "TBS0928" },
                    { new Guid("3a2b45af-6c2c-4420-9b93-c54476743324"), "E92000001", true, "TOTNES COMMUNITY HOSPITAL", "TBS0914" },
                    { new Guid("f572f90a-512c-4271-825b-ef6c82c28610"), "E92000001", true, "TOWERS HOSPITAL", "TBS0915" },
                    { new Guid("9dd22d58-44dc-4a73-9d52-e36cf488e570"), "E92000001", true, "TROWBRIDGE COMMUNITY HOSPITAL", "TBS0916" },
                    { new Guid("609bbae6-b46b-4e9f-bcc4-44bd5b01599a"), "E92000001", true, "TYNDALE CENTRE DAY HOSPITAL", "TBS0917" },
                    { new Guid("77b00623-0efd-4e7e-9cfd-b6a017cc9738"), "E92000001", true, "TYRELL HOSPITAL", "TBS0918" },
                    { new Guid("2a9c7e60-af14-4929-8233-79deb17855bd"), "E92000001", true, "UCKFIELD COMMUNITY HOSPITAL", "TBS0919" },
                    { new Guid("1be6ea32-ab5d-4efe-ac25-371d7b2525bd"), "E92000001", true, "WALLINGFORD COMMUNITY HOSPITAL", "TBS0929" },
                    { new Guid("6a939b10-83d6-4ecd-97df-c3f732f3f77f"), "E92000001", true, "UPTON DAY HOSPITAL [KENT]", "TBS0920" },
                    { new Guid("4b6fa7ab-b480-4651-8831-abc0f78755af"), "E92000001", true, "VERRINGTON HOSPITAL", "TBS0922" },
                    { new Guid("38872531-c7ec-4f84-b3c2-3d3f0c923c18"), "E92000001", true, "VICTORIA COTTAGE HOSPITAL [MARYPORT]", "TBS0923" },
                    { new Guid("d66633d4-3d4a-435c-a8ae-90845e79a6e7"), "E92000001", true, "VICTORIA HOSPITAL [DEAL]", "TBS0924" },
                    { new Guid("67320253-12f2-4c75-ba77-a916db405cb3"), "E92000001", true, "VICTORIA HOSPITAL [LICHFIELD]", "TBS0925" },
                    { new Guid("9623526c-0c4b-4ff2-a99d-4b8ed508a0ce"), "E92000001", true, "VICTORIA HOSPITAL [SIDMOUTH]", "TBS0926" },
                    { new Guid("6a0db87b-8a6e-48ab-964a-2e785cc3d6d3"), "E92000001", true, "VICTORIA INFIRMARY [CHESHIRE]", "TBS0927" },
                    { new Guid("b2a38a25-3fd9-4274-8ee5-f85744670ef1"), "E92000001", true, "UPTON HOUSE DAY HOSPITAL [NORTHAMPTON]", "TBS0921" },
                    { new Guid("3b110ed9-3faf-458e-a188-e6c6d719707c"), "E92000001", true, "TORRINGTON HOSPITAL", "TBS0913" },
                    { new Guid("148130af-457a-4509-946d-f6e6736f22ff"), "E92000001", true, "WEST MENDIP COMMUNITY HOSPITAL", "TBS0947" },
                    { new Guid("4424dc08-b730-4a64-b252-0b138dd52699"), "E92000001", true, "WEST PARK HOSPITAL [DARLINGTON]", "TBS0949" },
                    { new Guid("e1e68b14-02cc-40a2-b7db-0f3ec16ecc89"), "E92000001", true, "WILLOWBANK DAY HOSPITAL", "TBS0967" },
                    { new Guid("3e5952e3-4d49-4c01-8ac4-e91928af25a3"), "E92000001", true, "WILSON HOSPITAL", "TBS0968" },
                    { new Guid("1895e8b7-ab2c-4307-adf3-f36a119a32a4"), "E92000001", true, "WIMBOURNE COMMUNITY HOSPITAL", "TBS0969" },
                    { new Guid("3846f91b-107f-430a-badf-49fdf130079a"), "E92000001", true, "WINCHCOMBE HOSPITAL", "TBS0970" },
                    { new Guid("fa8fcdc3-4477-4b51-afc6-1deb64bf99fe"), "E92000001", true, "WINFIELD HOSPITAL", "TBS0971" },
                    { new Guid("30b7756e-486a-40c7-a24d-ff5ca4860db9"), "E92000001", true, "WITHERNSEA HOSPITAL", "TBS0972" },
                    { new Guid("397448bf-759f-4f96-bf78-a885710a2450"), "E92000001", true, "WILLITON HOSPITAL", "TBS0966" },
                    { new Guid("1ff7f5f2-1f35-47f0-8e3f-891eb4fdd2bd"), "E92000001", true, "WITNEY COMMUNITY HOSPITAL", "TBS0973" },
                    { new Guid("53737ff9-2b2e-4800-9851-5967dacd26d9"), "E92000001", true, "WOKINGHAM HOSPITAL", "TBS0975" },
                    { new Guid("2dfe6f2d-1a1e-45ca-82aa-cd19e85fdaee"), "E92000001", true, "WOODLAND HOSPITAL [KETTERING]", "TBS0976" },
                    { new Guid("1d6e8331-f3c2-4b5d-8415-e98dc82f72de"), "E92000001", true, "WOODLANDS HOSPITAL [DARLINGTON]", "TBS0977" },
                    { new Guid("6a9e42da-c4ba-48f9-a6ac-acc852027a87"), "E92000001", true, "WOODS HOSPITAL", "TBS0978" },
                    { new Guid("15e5950d-fc36-439d-aab0-ca520e0b617c"), "E92000001", true, "WORKINGTON COMMUNITY HOSPITAL", "TBS0979" },
                    { new Guid("9583590e-1925-4f46-9025-aeb6d5a04b07"), "W92000004", true, "WREXHAM CHEST CLINIC", "TBS0980" },
                    { new Guid("2599dd55-6274-41e1-ad21-3dbc0d2b8349"), "E92000001", true, "WOKING COMMUNITY HOSPITAL", "TBS0974" },
                    { new Guid("dbf3318e-ce56-4ff7-b3d5-ef84c846b9eb"), "E92000001", true, "WEST MIDLANDS HOSPITAL", "TBS0948" },
                    { new Guid("e36442ed-35cb-40e0-882c-1c5d41ba3f68"), "E92000001", true, "WILLIAM JULIEN COURTAULD HOSPITAL", "TBS0965" },
                    { new Guid("f937f259-3e02-4207-b62f-5528bc469424"), "E92000001", true, "WIGTON HOSPITAL", "TBS0964" },
                    { new Guid("f6af9dae-a163-419f-ade9-d4231621e518"), "E92000001", true, "WESTBURY COMMUNITY HOSPITAL", "TBS0950" },
                    { new Guid("629bfd57-7c1d-438e-9cdf-c1524f885090"), "E92000001", true, "WESTERN EYE HOSPITAL", "TBS0951" },
                    { new Guid("db5ec265-0e69-4987-9f3b-8632840fe10e"), "E92000001", true, "WESTMINSTER MEMORIAL HOSPITAL", "TBS0952" },
                    { new Guid("cad2c172-1c1b-4194-81ee-582deef55f14"), "E92000001", true, "WESTON PARK HOSPITAL", "TBS0953" },
                    { new Guid("8c00f2ef-4a63-4f7c-a264-552e5ea5c1f2"), "E92000001", true, "WEYBRIDGE COMMUNITY HOSPITAL", "TBS0954" },
                    { new Guid("3dbcf14f-400a-46b0-b179-1aa84e656724"), "E92000001", true, "WEYMOUTH COMMUNITY HOSPITAL", "TBS0955" },
                    { new Guid("172b8133-4b56-4858-982a-a2b8cb4cd46d"), "E92000001", true, "WILLESDEN HOSPITAL", "TBS0999" },
                    { new Guid("a8ea4caa-1e57-4a50-9817-c718dad73539"), "E92000001", true, "WHALLEY DRIVE CLINIC", "TBS0956" },
                    { new Guid("2e96d1a5-91d4-4f82-9b37-e090dbe091be"), "E92000001", true, "WHELLEY HOSPITAL", "TBS0958" },
                    { new Guid("e78dc003-3cb5-4af6-bac6-822748137109"), "E92000001", true, "WHITBY COMMUNITY HOSPITAL", "TBS0959" },
                    { new Guid("9dbbac50-5607-4e9b-b21c-a1fafdd1c36d"), "E92000001", true, "WHITCHURCH HOSPITAL [SHROPSHIRE]", "TBS0960" },
                    { new Guid("19835e7e-2d97-4ee5-833e-62a603ba2685"), "E92000001", true, "WHITE CROSS REHABILITATION HOSPITAL", "TBS0961" },
                    { new Guid("7eda8526-ce1a-4bba-98d3-32e6e3ca816e"), "E92000001", true, "WHITSTABLE & TANKERTON HOSPITAL", "TBS0962" },
                    { new Guid("ee749b92-c917-46da-adc3-b2a67faa98ed"), "E92000001", true, "WHITWORTH HOSPITAL", "TBS0963" },
                    { new Guid("631c5c01-a844-49eb-8cdf-7555416cab1f"), "E92000001", true, "WHARFEDALE GENERAL HOSPITAL", "TBS0957" },
                    { new Guid("4d5c6fdb-dc53-44d3-8800-6a6babc9ea35"), "E92000001", true, "SPIRE RODING HOSPITAL", "TBS0840" },
                    { new Guid("c2e2f1fe-7a7e-4f18-8c45-869a53dd6124"), "E92000001", true, "TONBRIDGE COTTAGE HOSPITAL", "TBS0912" },
                    { new Guid("35d8d223-8e22-4601-9abf-32b86bb8bc0c"), "E92000001", true, "TIVERTON AND DISTRICT HOSPITAL", "TBS0910" },
                    { new Guid("9d01552f-d274-4cbc-a4a6-a15e4d34a72c"), "E92000001", true, "ST GEORGES HOSPITAL [MORPETH]", "TBS0860" },
                    { new Guid("0e76ada3-8ccd-4426-a6d0-4b08663c38a5"), "E92000001", true, "ST HELENS REHABILITATION HOSPITAL [YORK]", "TBS0861" },
                    { new Guid("dc26d176-1277-4ce2-a1a9-4ccadd053e27"), "E92000001", true, "ST JOHNS HOSPITAL", "TBS0862" },
                    { new Guid("b9262d3f-b00c-47e4-a6fb-7891523fce51"), "E92000001", true, "ST LEONARDS HOSPITAL [RINGWOOD]", "TBS0863" },
                    { new Guid("0370a142-f5ed-4c1e-991c-5d92b2aacb40"), "E92000001", true, "ST LEONARDS HOSPITAL [SUDBURY]", "TBS0864" },
                    { new Guid("07c51540-3fb0-44d6-8c40-c98a4ff59ae2"), "E92000001", true, "ST LUKE'S HOSPITAL [HUDDERSFIELD]", "TBS0865" },
                    { new Guid("db2877fd-2b4b-402e-9964-af271f1cd9ae"), "E92000001", true, "ST GEORGES HOSPITAL [LINCOLN]", "TBS0859" },
                    { new Guid("e83fcec2-0045-4e25-b86d-95451479d08b"), "E92000001", true, "ST LUKE'S HOSPITAL [MIDDLESBROUGH]", "TBS0866" },
                    { new Guid("23aa321d-33d9-4b4f-ac38-f585a004b1d2"), "E92000001", true, "ST MARKS HOSPITAL [MAIDENHEAD]", "TBS0868" },
                    { new Guid("fc53ef0a-c36f-46d9-b6f0-f8f86cd9cb00"), "E92000001", true, "ST MARTINS HOSPITAL [BATH]", "TBS0869" },
                    { new Guid("36910905-2275-4316-beb5-241396e8b893"), "E92000001", true, "ST MARTINS HOSPITAL [CANTERBURY]", "TBS0870" },
                    { new Guid("9fa43296-0946-4e57-bb3c-1e599b6d611b"), "E92000001", true, "ST MARYS [GLOUCESTER]", "TBS0871" },
                    { new Guid("a2c4eace-1bef-4773-bdbc-d6fad216867b"), "E92000001", true, "ST MARY'S HOSPITAL [LEEDS]", "TBS0872" },
                    { new Guid("98f82c09-d243-4c39-8260-4874c71a0433"), "E92000001", true, "ST MARY'S HOSPITAL [MELTON MOWBRAY]", "TBS0873" },
                    { new Guid("795bb9c1-8d85-4b38-ad40-d77344139fc1"), "E92000001", true, "ST MARK'S HOSPITAL [HARROW]", "TBS0867" },
                    { new Guid("1ca2bb07-2fb7-4e0c-a853-0568d7452ba0"), "E92000001", true, "ST MARY'S HOSPITAL [SCARBOROUGH]", "TBS0874" },
                    { new Guid("a54f2a19-1469-40d9-8913-799107453654"), "E92000001", true, "ST GEMMA'S HOSPICE", "TBS0858" },
                    { new Guid("40bcdeef-7d37-4632-9893-70f454a50045"), "E92000001", true, "ST EDMUNDS HOSPITAL [BURY]", "TBS0856" },
                    { new Guid("2332b4f9-b6c0-42a5-86d6-eca9837d40a7"), "E92000001", true, "SPIRE SUSSEX HOSPITAL", "TBS0842" },
                    { new Guid("292d7a2b-1b87-4644-ad43-83f1301a0c94"), "E92000001", true, "SPIRE THAMES VALLEY HOSPITAL", "TBS0843" },
                    { new Guid("2c216f57-6c1a-4e50-811a-5f8a5b8568b3"), "E92000001", true, "SPIRE WASHINGTON HOSPITAL", "TBS0844" },
                    { new Guid("7c1c7f89-d326-42ec-8edd-d67cd3e52488"), "E92000001", true, "SPRINGFIELD HOSPITAL", "TBS0845" },
                    { new Guid("e26d81f7-1557-4193-bc39-0edb829e0f37"), "E92000001", true, "ST ANDREWS [WELLS]", "TBS0846" },
                    { new Guid("d719e975-644b-4fda-b001-c77ab132a672"), "E92000001", true, "ST ANDREW'S HOSPITAL [LONDON]", "TBS0847" },
                    { new Guid("b30f30f7-51e8-4805-b1d3-6d719a0b5b9a"), "E92000001", true, "ST EDMUND'S HOSPITAL [NORTHAMPTON]", "TBS0857" },
                    { new Guid("fd883e81-6f40-4d86-a7b0-776a1b39bc75"), "E92000001", true, "ST ANNE'S HOSPITAL [ALTRINCHAM]", "TBS0848" },
                    { new Guid("31140ae9-27d7-4860-833b-673dab14fd0e"), "E92000001", true, "ST AUSTELL COMMUNITY HOSPITAL", "TBS0850" },
                    { new Guid("28dcb0dc-31d4-4b7a-ad44-3e27cb37016c"), "E92000001", true, "ST BARNABAS HOSPITAL", "TBS0851" },
                    { new Guid("e0fd498c-85f1-49d2-a283-0dcc0dc367b6"), "E92000001", true, "ST BARTHOLOMEWS DAY HOSPITAL [LIVERPOOL]", "TBS0852" },
                    { new Guid("11e3674c-17d3-4c4e-8f2c-a4af70dd6fb0"), "E92000001", true, "ST CATHERINES HOSPITAL", "TBS0853" },
                    { new Guid("4745febe-293f-40ec-bc10-8490a4b245cd"), "E92000001", true, "ST CHARLES HOSPITAL", "TBS0854" },
                    { new Guid("9ba3ecfd-157a-418d-9fe4-5fc83add48b0"), "E92000001", true, "ST CHRISTOPHER'S HOSPITAL", "TBS0855" },
                    { new Guid("dc37346d-f34c-451b-90f0-35b5d69aae46"), "E92000001", true, "ST ANN'S HOSPITAL [POOLE]", "TBS0849" },
                    { new Guid("a5f820dd-37d5-4ef6-88d1-d779183c306b"), "E92000001", true, "TOLWORTH HOSPITAL", "TBS0911" },
                    { new Guid("695d7449-a7bf-46dd-9e7e-6d49f05c24d1"), "E92000001", true, "ST MARY'S HOSPITAL [ST MARY'S]", "TBS0875" },
                    { new Guid("a661eb9f-262c-49e8-bc53-1010973021ab"), "E92000001", true, "ST MICHAEL'S HOSPITAL [HAYLE]", "TBS0877" },
                    { new Guid("de3ccd70-38df-460b-9a20-e0857c55c292"), "E92000001", true, "SUTTON HOSPITAL", "TBS0896" },
                    { new Guid("20f1a936-225b-4b47-9ecd-cedc4fb2fd45"), "E92000001", true, "SWANAGE COMMUNITY HOSPITAL", "TBS0897" },
                    { new Guid("391fbb68-4531-4f6c-9e8d-b79b8ca93608"), "E92000001", true, "SYLVAN HOSPITAL", "TBS0898" },
                    { new Guid("4f24dd69-5bc8-4f1c-99f5-2260817643a6"), "E92000001", true, "TARPORLEY WAR MEMORIAL HOSPITAL", "TBS0899" },
                    { new Guid("f934b137-6e22-4dc8-bcc4-272f75d7f6df"), "E92000001", true, "TAVISTOCK HOSPITAL", "TBS0900" },
                    { new Guid("e310a5bd-842e-4298-a162-667abb8e9dd2"), "E92000001", true, "TEDDINGTON MEMORIAL HOSPITAL", "TBS0901" },
                    { new Guid("04dd6731-97a7-4fad-bfde-d95c98cd04c5"), "E92000001", true, "SURBITON HOSPITAL", "TBS0895" },
                    { new Guid("0a8ab02a-c64b-459e-8ff8-1f96bb8a0159"), "E92000001", true, "TEIGNMOUTH HOSPITAL", "TBS0902" },
                    { new Guid("dbd45055-c1c6-46f5-828f-392e9c9ba04b"), "E92000001", true, "TEWKESBURY GENERAL HOSPITAL", "TBS0904" },
                    { new Guid("2e04b26e-1e81-4022-8d4e-f3817c3fbe47"), "E92000001", true, "THAME COMMUNITY HOSPITAL", "TBS0905" },
                    { new Guid("b787c819-9215-4757-80e6-5349c10d1564"), "E92000001", true, "THORNBURY HOSPITAL [BRISTOL]", "TBS0906" },
                    { new Guid("533f62df-234b-41a9-8735-27b0849fa018"), "E92000001", true, "THORNBURY HOSPITAL [SHEFFIELD]", "TBS0907" },
                    { new Guid("04248670-4d1f-4026-bf22-e913208e672e"), "E92000001", true, "THREE SHIRES HOSPITAL", "TBS0908" },
                    { new Guid("26b4b173-8990-41c6-8099-405d26f158d2"), "E92000001", true, "THURROCK COMMUNITY HOSPITAL", "TBS0909" },
                    { new Guid("2a1aede9-90b4-4967-84c7-e2788fde1eec"), "E92000001", true, "TENBURY COMMUNITY HOSPITAL", "TBS0903" },
                    { new Guid("1cfd833f-76ca-41e5-b151-65045df09358"), "E92000001", true, "ST MICHAEL'S HOSPITAL [BRISTOL]", "TBS0876" },
                    { new Guid("cc66f1f9-f212-4007-9797-bc61d23eb847"), "E92000001", true, "SUNDERLAND EYE INFIRMARY", "TBS0894" },
                    { new Guid("a5aa2617-09b7-4889-a863-2dfa6d254236"), "E92000001", true, "STROUD GENERAL HOSPITAL", "TBS0892" },
                    { new Guid("768cfc30-fc07-420f-86ce-d6f91501d253"), "E92000001", true, "ST MONICAS HOSPITAL", "TBS0878" },
                    { new Guid("9e2bd90d-dd34-4355-af2a-2917065629f0"), "E92000001", true, "ST NICHOLAS HOSPITAL", "TBS0879" },
                    { new Guid("c0185c82-7014-4a10-85a5-a644cbf4572b"), "E92000001", true, "ST OSWALDS HOSPITAL", "TBS0880" },
                    { new Guid("5d75da70-d99e-46d6-a066-97347f6ee5c4"), "E92000001", true, "ST PAUL'S HOSPITAL", "TBS0881" },
                    { new Guid("c58dfe16-bcf6-4380-97e2-a2250c63b41d"), "E92000001", true, "ST PETER'S HOSPITAL [MALDON]", "TBS0882" },
                    { new Guid("fbf5eed1-36ed-49ef-8205-89fc822b3f28"), "E92000001", true, "ST THOMAS HOSPITAL [STOCKPORT]", "TBS0883" },
                    { new Guid("9a08a374-5112-4386-9ea1-100819754d8b"), "E92000001", true, "STROUD MATERNITY HOSPITAL", "TBS0893" },
                    { new Guid("fa7f0d01-6347-41c3-b183-e504eb17cc6c"), "E92000001", true, "STAMFORD & RUTLAND HOSPITAL", "TBS0884" },
                    { new Guid("26c2e669-97e7-4e78-97e2-b87af2729ea7"), "E92000001", true, "STEAD PRIMARY CARE HOSPITAL", "TBS0886" },
                    { new Guid("71259dbf-b9b8-46ed-a7bd-9c256bdf848b"), "E92000001", true, "STEWART DAY HOSPITAL", "TBS0887" },
                    { new Guid("458809a5-5ae8-4d9d-8af3-d4249d14a5de"), "E92000001", true, "STONE HOUSE HOSPITAL", "TBS0888" },
                    { new Guid("3fa1136d-a326-49e0-907f-8e12db7f8429"), "E92000001", true, "STONEBURY DAY HOSPITAL", "TBS0889" },
                    { new Guid("d5e9268f-e45e-48cd-aa4d-35a846fecb45"), "E92000001", true, "STRATFORD HOSPITAL", "TBS0890" },
                    { new Guid("4e9c7a1d-a42d-414a-9d14-4e2986c939f0"), "E92000001", true, "STRATTON HOSPITAL", "TBS0891" },
                    { new Guid("818340c5-52a3-44c5-be80-957033a3dcda"), "E92000001", true, "STANDISH HOSPITAL", "TBS0885" },
                    { new Guid("a8853a77-64b4-4eea-b3a1-017d2309d8b0"), "E92000001", true, "NORWICH COMMUNITY HOSPITAL", "TBS0702" },
                    { new Guid("27bb6d5a-ffb2-448c-8583-19dd3d471c02"), "E92000001", true, "NORTHGATE HOSPITAL [MORPETH]", "TBS0701" },
                    { new Guid("f98655e6-a6cc-4759-8e6f-5bb39dd74403"), "E92000001", true, "NORTHGATE HOSPITAL [GREAT YARMOUTH]", "TBS0700" },
                    { new Guid("121c9756-16e7-4186-a1d4-e36b51b57dfb"), "E92000001", true, "CHERRY KNOWLE HOSPITAL", "TBS0514" },
                    { new Guid("9e12885c-7511-4266-8eb9-19175f116167"), "E92000001", true, "CHERRY TREE HOSPITAL", "TBS0515" },
                    { new Guid("c2e5388f-a002-4e74-b924-ae34eb7fd6b0"), "E92000001", true, "CHESHUNT COMMUNITY HOSPITAL", "TBS0516" },
                    { new Guid("4513e509-0e43-4c2f-b4b4-9abd4d81a856"), "E92000001", true, "CHINGFORD HOSPITAL", "TBS0517" },
                    { new Guid("f0d93c98-3a4d-4efd-90b6-70d7cfb25617"), "E92000001", true, "CHIPPENHAM COMMUNITY HOSPITAL", "TBS0518" },
                    { new Guid("2e32407c-b568-447f-8c4a-052dd3a1fd67"), "E92000001", true, "CHIPPING NORTON HOSPITAL", "TBS0519" },
                    { new Guid("379d9d0f-a629-4d21-bdbb-1702f2960ee8"), "E92000001", true, "CHELMSFORD & ESSEX HOSPITAL", "TBS0513" },
                    { new Guid("eda85b84-4400-49fe-a011-cc06ea57aee3"), "E92000001", true, "CIRENCESTER HOSPITAL", "TBS0520" },
                    { new Guid("fe6a5f2d-0389-45de-8391-56d5b0f65374"), "E92000001", true, "CLEVEDON HOSPITAL", "TBS0522" },
                    { new Guid("312b9df1-b85b-4b11-94e8-4b2034125b3f"), "E92000001", true, "CLIFTON HOSPITAL", "TBS0523" },
                    { new Guid("68806afc-e4a9-472b-8899-ba0426b1ebe5"), "E92000001", true, "CLITHEROE COMMUNITY HOSPITAL", "TBS0524" },
                    { new Guid("3f1725c7-758a-42a4-ab5d-21ee6ad2c8d5"), "E92000001", true, "COCKERMOUTH COMMUNITY HOSPITAL", "TBS0525" },
                    { new Guid("2b5f6287-4b9f-4eb6-97c3-abbbfbfeb84e"), "E92000001", true, "CONGLETON WAR MEMORIAL HOSPITAL", "TBS0984" },
                    { new Guid("778a2fae-8eb6-440b-b2c6-056b637b94e7"), "E92000001", true, "COOKRIDGE HOSPITAL", "TBS0526" },
                    { new Guid("17340cde-67c7-4203-9957-afa0c3275895"), "E92000001", true, "CLAYTON HOSPITAL", "TBS0521" },
                    { new Guid("3b194a7c-455b-4dbd-a9c7-95acc378e68e"), "E92000001", true, "COQUETDALE COTTAGE HOSPITAL", "TBS0527" },
                    { new Guid("60593be6-cc9f-4a86-bbc0-f65da4b79b51"), "E92000001", true, "CHEADLE ROYAL HOSPITAL", "TBS0512" },
                    { new Guid("5ab01d61-cf4b-46ec-8792-44632e8a7e4c"), "E92000001", true, "CHATSWORTH SUITE", "TBS0510" },
                    { new Guid("6385964d-19af-4fb0-ac9f-122a64305752"), "E92000001", true, "CAPIO OAKS HOSPITAL", "TBS0496" },
                    { new Guid("c07e2713-e938-4f52-92ff-ef19624eb235"), "E92000001", true, "CAPIO READING HOSPITAL", "TBS0497" },
                    { new Guid("383386f0-421f-46d6-b037-b73b44be95ad"), "E92000001", true, "CAPIO RIVERS HOSPITAL", "TBS0498" },
                    { new Guid("023f1000-b4d1-4887-b601-333a87bb6514"), "E92000001", true, "CARLTON HEALTH CLINIC", "TBS0499" },
                    { new Guid("3ed63450-486c-4ade-a188-07f6c9e8139d"), "E92000001", true, "CASTLEBERG HOSPITAL", "TBS0500" },
                    { new Guid("a4ddff83-2c02-4377-a034-0304e5e3c374"), "E92000001", true, "CASTLEFORD & NORMANTON DISTRICT HOSPITAL", "TBS0501" },
                    { new Guid("dd30aad5-4955-43fc-806e-3700ed14257e"), "E92000001", true, "CHEADLE HOSPITAL- NORTH STAFFS COMBINED HEALTHCARE", "TBS0511" },
                    { new Guid("7e12c815-5491-4cf9-aa03-5f4528aed0e4"), "E92000001", true, "CATERHAM DENE HOSPITAL", "TBS0502" },
                    { new Guid("b0dd2ccd-a351-4621-92e7-8adacb8e55b4"), "E92000001", true, "CHADWELL HEATH HOSPITAL", "TBS0504" },
                    { new Guid("214ec28c-2a69-4940-89ef-136bc462dfbe"), "E92000001", true, "CHALFONT'S & GERRARDS CROSS HOSPITAL", "TBS0505" },
                    { new Guid("97e4ac81-920c-4877-9151-96b8aef40c36"), "E92000001", true, "CHANTRY HOUSE DAY HOSPITAL", "TBS0506" },
                    { new Guid("1a498852-ef37-4a94-9a87-44fba3869b70"), "E92000001", true, "CHAPEL ALLERTON HOSPITAL", "TBS0507" },
                    { new Guid("e7cf1925-02b2-4761-a526-98733667e908"), "E92000001", true, "CHARD & DISTRICT HOSPITAL", "TBS0508" },
                    { new Guid("f7d4b02a-494c-466e-b7d6-9a11cb09267a"), "E92000001", true, "CHASE HOSPITAL", "TBS0509" },
                    { new Guid("9a45c03f-ab96-45a7-8f11-36c4bb210f60"), "E92000001", true, "CAVENDISH HOSPITAL", "TBS0503" },
                    { new Guid("265ba139-52f9-4207-b10a-feedb861cf85"), "E92000001", true, "CALDERSTONES HOSPITAL", "TBS0495" },
                    { new Guid("a6c371f6-a52e-4821-bb68-c59e7f0a3b1e"), "E92000001", true, "CORBY COMMUNITY HOSPITAL", "TBS0528" },
                    { new Guid("85364a25-a67d-4194-83f0-d48e848b3fac"), "E92000001", true, "COSSHAM HOSPITAL", "TBS0530" },
                    { new Guid("80ffe3cc-baba-4785-963c-81b6a30a217d"), "E92000001", true, "DOVE DAY HOSPITAL", "TBS0549" },
                    { new Guid("24766cb2-0f78-4c8f-a188-57f1e3fe9a30"), "E92000001", true, "DRYDEN ROAD DAY HOSPITAL", "TBS0550" },
                    { new Guid("131ccafa-9147-4bd3-93f4-157c70b56b4b"), "E92000001", true, "DUCHY HOSPITAL", "TBS0551" },
                    { new Guid("3074f823-eb92-47b5-b434-75b860c5d7cc"), "E92000001", true, "DUNEDIN HOSPITAL", "TBS0552" },
                    { new Guid("b2efe4a8-51f0-452b-9a9b-6b6d4ce1ca69"), "E92000001", true, "DURHAM COMMUNITY HOSPITAL", "TBS0553" },
                    { new Guid("f835d1ab-0e33-4291-b567-60aac42f8c6e"), "E92000001", true, "ECH - EAST CLEVELAND HOSPITAL", "TBS0554" },
                    { new Guid("4a1177f9-6a35-43e9-b106-c309bd5313dd"), "E92000001", true, "DORKING GENERAL HOSPITAL", "TBS0548" },
                    { new Guid("b700b31e-6d6d-4ee5-90c7-5680b8cf34d7"), "E92000001", true, "EDENBRIDGE WAR MEMORIAL HOSPITAL", "TBS0555" },
                    { new Guid("13b6c143-587d-46f8-ba7b-5341482066f4"), "E92000001", true, "EDITH CAVELL HOSPITAL", "TBS0557" },
                    { new Guid("80948edb-30fa-4d22-8f2c-c9bcc886c6ee"), "E92000001", true, "EDWARD HAIN HOSPITAL", "TBS0558" },
                    { new Guid("17f93ffc-01e5-48e1-b1fd-10fc7308f9ec"), "E92000001", true, "ELDERLY DAY HOSPITAL", "TBS0559" },
                    { new Guid("a25a8a6c-4223-437e-a1a4-96894227549c"), "E92000001", true, "ELIZABETH GARRETT ANDERSON HOSPITAL", "TBS0560" },
                    { new Guid("14476a1d-6db0-49ae-bf4d-c0d95b51af21"), "E92000001", true, "ELLEN BADGER HOSPITAL", "TBS0561" },
                    { new Guid("7aaec4ba-fee4-4db5-8498-df3987bb620f"), "E92000001", true, "ELLESMERE PORT HOSPITAL", "TBS0562" },
                    { new Guid("de747d91-aa94-411e-a7c0-55873c96ad2c"), "E92000001", true, "EDGBASTON HOSPITAL", "TBS0556" },
                    { new Guid("75a8d82b-93a2-411f-b082-b26fde40217d"), "E92000001", true, "CORONATION HOSPITAL", "TBS0529" },
                    { new Guid("db51d970-e418-4755-b7cd-beedeb36137f"), "E92000001", true, "DILKE MEMORIAL HOSPITAL", "TBS0547" },
                    { new Guid("1ebcb983-55d2-4365-ae0f-c91d1d19ef86"), "E92000001", true, "DEVONSHIRE ROAD HOSPITAL", "TBS0545" },
                    { new Guid("3871aa2d-b501-4e53-8d6e-346beebe9167"), "E92000001", true, "CRANLEIGH HOSPITAL", "TBS0531" },
                    { new Guid("62be61a4-7d43-415b-baae-92e83e521140"), "E92000001", true, "CRANLEIGH VILLAGE HOSPITAL", "TBS0532" },
                    { new Guid("d2207ecd-576b-43b9-9a26-bbe647c96c0a"), "E92000001", true, "CREDITON HOSPITAL", "TBS0533" },
                    { new Guid("87f31e3d-d28f-4f61-8b8c-17ffef38e149"), "E92000001", true, "CREWKERNE HOSPITAL", "TBS0534" },
                    { new Guid("c910f3d0-3549-4ae8-be9b-3a1ee795b22e"), "E92000001", true, "CROMER HOSPITAL", "TBS0535" },
                    { new Guid("e78d3e47-1f78-4873-bef0-105912a4a28a"), "E92000001", true, "CROSS LANE HOSPITAL", "TBS0536" },
                    { new Guid("e88b61ba-4d65-4a7c-b7b2-833a86472322"), "E92000001", true, "DIDCOT COMMUNITY HOSPITAL", "TBS0546" },
                    { new Guid("9e4fd059-9f62-4e75-b049-33bf60d7e594"), "E92000001", true, "CROWBOROUGH WAR MEMORIAL HOSPITAL", "TBS0537" },
                    { new Guid("db50fe9a-3561-4848-8aa7-f04ef2f69fa2"), "E92000001", true, "DARTMOUTH HOSPITAL", "TBS0539" },
                    { new Guid("c562c7b6-2cf8-4ec1-8cb2-2fd0d40169aa"), "E92000001", true, "DAWLISH HOSPITAL", "TBS0540" },
                    { new Guid("ff098a16-6549-4b31-ae3f-e23f5d68713e"), "E92000001", true, "DELLWOOD HOSPITAL", "TBS0541" },
                    { new Guid("1a2ad18f-94ef-4c63-ab09-40ac192343c7"), "E92000001", true, "DENMARK ROAD DAY HOSPITAL", "TBS0542" },
                    { new Guid("29f693f4-4738-4163-a6a5-71005d9601bb"), "E92000001", true, "DEREHAM HOSPITAL", "TBS0543" },
                    { new Guid("868dbe9d-8a26-4e06-ae43-9abeb6202aec"), "E92000001", true, "DEVIZES COMMUNITY HOSPITAL", "TBS0544" },
                    { new Guid("ae1bc8c1-6025-455b-900d-9476965e1904"), "E92000001", true, "DANETRE HOSPITAL", "TBS0538" },
                    { new Guid("8218a639-27b1-4a8c-8f44-3ca722294c4f"), "E92000001", true, "ELMS DAY HOSPITAL", "TBS0563" },
                    { new Guid("d059dd3f-7ee1-4fd3-bdd1-56034e08e99f"), "E92000001", true, "BUXTON HOSPITAL", "TBS0494" },
                    { new Guid("4b61f76e-b9a2-4aef-a403-95a0e922b603"), "E92000001", true, "BURNHAM ON SEA WAR MEMORIAL HOSPITAL", "TBS0492" },
                    { new Guid("b2b3ca4a-fa44-4040-8ee5-44b05a56628c"), "E92000001", true, "AXMINSTER HOSPITAL", "TBS0443" },
                    { new Guid("16908b7a-58f8-4e17-a8e8-91888d86f52f"), "E92000001", true, "BARKING HOSPITAL", "TBS0983" },
                    { new Guid("919633a7-e62d-490f-8e4b-84339da0ab9b"), "E92000001", true, "BARROW HOSPITAL", "TBS0444" },
                    { new Guid("db5d0848-c1cd-4e30-a28c-a6199621174c"), "E92000001", true, "BARROWBY HOUSE", "TBS0445" },
                    { new Guid("33ee2791-894f-45b2-880c-e9b83854fd07"), "E92000001", true, "BATH MINERAL HOSPITAL", "TBS0446" },
                    { new Guid("b74e4e2f-f51f-4fd3-8167-9ad2c8e588eb"), "E92000001", true, "BATH ROAD DAY HOSPITAL", "TBS0447" },
                    { new Guid("bd24c256-a61d-4c8e-b656-0f90bd2cff3d"), "E92000001", true, "AVENUE DAY HOSPITAL", "TBS0442" },
                    { new Guid("d016bc16-ed2e-43cd-837d-8867a4435c99"), "E92000001", true, "BEACON DAY HOSPITAL", "TBS0448" },
                    { new Guid("60d1768d-6d86-4a22-adc9-220b6a075308"), "E92000001", true, "BEAUMONT HOSPITAL", "TBS0450" },
                    { new Guid("1387c5d9-b653-43bd-93fc-20ce75edd420"), "E92000001", true, "BECCLES & DISTRICT HOSPITAL", "TBS0451" },
                    { new Guid("fc9372fa-0d90-4257-9b2f-e4a80aaee992"), "E92000001", true, "BECKENHAM HOSPITAL", "TBS0452" },
                    { new Guid("b033556e-8bd5-4277-bf79-9875774dc403"), "E92000001", true, "BEIGHTON COMMUNITY HOSPITAL (THE CHILD & FAMILY THERAPY TEAM)", "TBS0453" },
                    { new Guid("a72c02db-34ae-4be2-96eb-7ed3f992499d"), "E92000001", true, "BENSHAM HOSPITAL", "TBS0454" },
                    { new Guid("aa0aa029-25ab-41ee-9414-46b1c6d6c238"), "E92000001", true, "BERKELEY HOSPITAL", "TBS0455" },
                    { new Guid("54b0bd76-51e1-484a-8ad8-deeaa419d564"), "E92000001", true, "BEARDWOOD HOSPITAL", "TBS0449" },
                    { new Guid("be836647-61ad-4068-824d-06198752c7e0"), "E92000001", true, "BETHLEM ROYAL HOSPITAL", "TBS0456" },
                    { new Guid("c4460d63-44e4-412c-8de9-b052514e41b5"), "E92000001", true, "AUCKLAND PARK HOSPITAL", "TBS0441" },
                    { new Guid("b5cee958-0e64-41a4-9f66-bc008c7a59e9"), "E92000001", true, "ASHWORTH HOSPITAL", "TBS0439" },
                    { new Guid("a82e9405-d427-4db3-9a4b-5292eb76f31e"), "E92000001", true, "ACRE DAY HOSPITAL", "TBS0425" },
                    { new Guid("eea9a534-f4b5-457b-a8bb-73a2d40330b6"), "E92000001", true, "ADAMS DAY HOSPITAL", "TBS0426" },
                    { new Guid("2a4164c7-4487-4e33-acb1-7cd51f7ad158"), "E92000001", true, "AINTREE HOSPITALS - OPD", "TBS0427" },
                    { new Guid("f5d814fc-d3a5-4b3b-af84-2895a05815f1"), "E92000001", true, "ALDEBURGH HOSPITAL", "TBS0428" },
                    { new Guid("e9ca976c-fa94-4525-9eaa-f8f4f550249e"), "E92000001", true, "ALDERNEY HOSPITAL", "TBS0429" },
                    { new Guid("5786ff70-02fe-4d20-bad7-affd201fe419"), "E92000001", true, "ALDRINGTON DAY HOSPITAL", "TBS0430" },
                    { new Guid("87ac9a14-5333-4de8-bac9-cbc1d5bf86a6"), "E92000001", true, "ASTLEY HOSPITAL", "TBS0440" },
                    { new Guid("8bac3ff5-fd44-4989-b4e7-0b1846686eb3"), "E92000001", true, "ALFRED BEAN HOSPITAL", "TBS0431" },
                    { new Guid("7877b857-c94d-47c1-9636-e0ef6005ef89"), "E92000001", true, "AMBERSTONE HOSPITAL", "TBS0433" },
                    { new Guid("bfa31f50-c61d-4ff9-b92a-193242ebf89e"), "E92000001", true, "ARUNDAL HOSPITAL LODGE", "TBS0434" },
                    { new Guid("80608b56-dc0e-4205-ad07-5fbbc878d679"), "E92000001", true, "ASH ETON", "TBS0435" },
                    { new Guid("18b079d8-ba76-4145-930b-547d4c3b623f"), "E92000001", true, "ASHBURTON AND BUCKFASTLEIGH HOSPITAL", "TBS0436" },
                    { new Guid("51f4291e-dddd-497d-bd04-158763e1a131"), "E92000001", true, "ASHINGTON HOSPITAL", "TBS0437" },
                    { new Guid("c1c90529-f7d0-40d0-b644-9c15e3046c25"), "E92000001", true, "ASHTEAD HOSPITAL", "TBS0438" },
                    { new Guid("f370618c-e33a-4663-90fc-7c986e4053ae"), "E92000001", true, "ALTON COMMUNITY HOSPITAL", "TBS0432" },
                    { new Guid("d6bf4347-d6c1-49c2-9eac-f3cb57e082cc"), "E92000001", true, "BUTLEIGH HOSPITAL", "TBS0493" },
                    { new Guid("d2563ac7-9d6e-428f-8b4e-6cd1254f2612"), "E92000001", true, "BEXHILL COMMUNITY HOSPITAL", "TBS0457" },
                    { new Guid("6050e0bd-f829-403f-83f2-6ff2865b1f2d"), "E92000001", true, "BIDEFORD AND DISTRICT HOSPITAL", "TBS0459" },
                    { new Guid("1bdbc2b9-1971-4af8-ac2a-d3a8f06b8399"), "E92000001", true, "BRAMCOTE HOSPITAL", "TBS0478" },
                    { new Guid("476fec49-f090-42f4-9c7c-058e1c10e226"), "E92000001", true, "BRAMPTON WAR MEMORIAL HOSPITAL", "TBS0479" },
                    { new Guid("c9235f5d-775f-40de-aa3b-c9dec5678d73"), "E92000001", true, "BRENTFORD HOSPITAL", "TBS0480" },
                    { new Guid("9df0c003-65cb-4c9b-a667-8eba40d5fe62"), "E92000001", true, "BRIDGEWAYS DAY HOSPITAL", "TBS0481" },
                    { new Guid("097ea09c-71a4-4178-bf54-c2858ad9d493"), "E92000001", true, "BRIDGWATER HOSPITAL", "TBS0482" },
                    { new Guid("bb4484b3-b0ca-4e63-88f6-cb84192835b2"), "E92000001", true, "BRIDPORT COMMUNITY HOSPITAL", "TBS0483" },
                    { new Guid("5c0068e6-fa8e-43de-ad7e-6d2b7c497e10"), "E92000001", true, "BRADWELL HOSPITAL", "TBS0477" },
                    { new Guid("303e9e42-6d29-43a2-858c-e67554ba4ed9"), "E92000001", true, "BRISTOL GENERAL HOSPITAL", "TBS0484" },
                    { new Guid("6fe27200-3464-48af-99f3-745c6a868089"), "E92000001", true, "BRIXHAM HOSPITAL", "TBS0486" },
                    { new Guid("eeb163a4-245b-403d-838c-f03b4e195b5c"), "E92000001", true, "BROMYARD COMMUNITY HOSPITAL", "TBS0487" },
                    { new Guid("2c8ba49a-64a7-43fa-81eb-a072b2a0253b"), "E92000001", true, "BROOKLANDS HOSPITAL", "TBS0488" },
                    { new Guid("87ec205d-9067-4c55-b3b2-0595705af675"), "E92000001", true, "BUCKNALL HOSPITAL", "TBS0489" },
                    { new Guid("c39362be-0a0c-440a-a471-5daa4d41b33a"), "E92000001", true, "BUDOCK HOSPITAL", "TBS0490" },
                    { new Guid("fcecc812-b678-42b4-9273-d7766014e515"), "E92000001", true, "BURDEN NEUROLOGICAL HOSPITAL", "TBS0491" },
                    { new Guid("cf060c72-425b-47ac-a892-891658b59481"), "E92000001", true, "BRISTOL HOMOEOPATHIC HOSPITAL", "TBS0485" },
                    { new Guid("4987e982-7774-42ac-b22f-a24bdf5e2c22"), "E92000001", true, "BICESTER COTTAGE HOSPITAL", "TBS0458" },
                    { new Guid("1c39b75b-ba22-4574-afc0-459c0009d4b7"), "E92000001", true, "BRACKLEY COTTAGE HOSPITAL", "TBS0476" },
                    { new Guid("9857ff70-d992-4e8d-b97d-d174b70dfb74"), "E92000001", true, "BOVEY TRACEY HOSPITAL", "TBS0474" },
                    { new Guid("8d01039e-986e-4e26-ad3e-2e30729da071"), "E92000001", true, "BILLINGE HOSPITAL", "TBS0460" },
                    { new Guid("86101239-4962-40a4-90f3-48d6fd2bf8e2"), "E92000001", true, "BINGLEY HOSPITAL", "TBS0461" },
                    { new Guid("a13d2e29-6f40-488f-8fc4-ba84101cfe0f"), "E92000001", true, "BIRKDALE CLINIC (CROSBY)", "TBS0462" },
                    { new Guid("f6558ad2-1df5-4d80-8f5d-17fa2235b97b"), "E92000001", true, "BIRMINGHAM WOMENS HOSPITAL", "TBS0463" }
                });

            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "IsLegacy", "Name", "TBServiceCode" },
                values: new object[,]
                {
                    { new Guid("c80bb3a7-eef4-48a6-aeed-a3e6ea055b17"), "E92000001", true, "BISHOPS WOOD HOSPITAL", "TBS0464" },
                    { new Guid("e3792b8f-19f7-4ae1-ad55-43655ae27012"), "E92000001", true, "BLACKBERRY HILL HOSPITAL", "TBS0465" },
                    { new Guid("70f2cca0-2289-4482-9ebf-0be8e031203f"), "E92000001", true, "BOWOOD DAY HOSPITAL", "TBS0475" },
                    { new Guid("904ac274-f26b-4b8a-9fe3-8ab27cca3e9f"), "E92000001", true, "BLANDFORD COMMUNITY HOSPITAL", "TBS0466" },
                    { new Guid("a63548af-fe08-47cf-acd0-d9ea702b4e0c"), "E92000001", true, "BODMIN HOSPITAL", "TBS0468" },
                    { new Guid("1b917fe6-3fb7-4fba-b5f5-744c69f9ca60"), "E92000001", true, "BOLINGBROKE HOSPITAL", "TBS0469" },
                    { new Guid("e738594b-19ef-4a88-9dbf-a5a7a5ff443a"), "E92000001", true, "BOLITHO HOSPITAL", "TBS0470" },
                    { new Guid("cdfa84d5-e31d-4920-bff0-53cbc78142f4"), "E92000001", true, "BOOTHAM PARK HOSPITAL", "TBS0471" },
                    { new Guid("d0424f82-f884-450e-b259-821f08f1c29f"), "E92000001", true, "BOSCOMBE COMMUNITY HOSPITAL", "TBS0472" },
                    { new Guid("e8324d8b-03ab-47c6-8385-514fac6dca6a"), "E92000001", true, "BOURNE HEALTH CLINIC", "TBS0473" },
                    { new Guid("49ae657f-2a66-4674-8aba-060bc6388a08"), "E92000001", true, "BLT PRIVATE HOSPITALS", "TBS0467" },
                    { new Guid("f8d1338d-9db8-4866-b060-c3ca4a7a3d08"), "E92000001", true, "EMSWORTH HOSPITAL", "TBS0564" },
                    { new Guid("bc9d011b-b150-4d67-843f-d52fcc7c026a"), "E92000001", true, "EPSOM GENERAL HOSPITAL", "TBS0985" },
                    { new Guid("509797da-05a6-4c9e-8168-d46a7f22d867"), "E92000001", true, "ESPERANCE PRIVATE HOSPITAL", "TBS0565" },
                    { new Guid("f16b7c49-8180-438f-a114-9a2e848cac52"), "E92000001", true, "LYDNEY & DISTRICT HOSPITAL", "TBS0651" },
                    { new Guid("40c7827b-9556-492f-af7b-f2c4a5a3f1f2"), "E92000001", true, "LYTHAM HOSPITAL", "TBS0652" },
                    { new Guid("9f8eb5c4-0fc0-4c84-bfdb-eed9b1d9cc94"), "E92000001", true, "MALHAM HOUSE DAY HOSPITAL", "TBS0653" },
                    { new Guid("c6f1ace8-1305-4cd6-a1ad-db34fca5105f"), "E92000001", true, "MALMESBURY COMMUNITY HOSPITAL", "TBS0654" },
                    { new Guid("d30ce642-60ee-4c35-943e-3a5ca2075b72"), "E92000001", true, "MALTON COMMUNITY HOSPITAL", "TBS0655" },
                    { new Guid("33464912-e5b1-4998-afca-083c3ae65a80"), "E92000001", true, "MALVERN COMMUNITY HOSPITAL", "TBS0656" },
                    { new Guid("5f17d1c6-6964-447a-aaa5-791e8895b8b0"), "E92000001", true, "LOWESTOFT & NORTH SUFFOLK HOSPITAL", "TBS0650" },
                    { new Guid("656a1449-4e9b-4dee-bf0f-8c6d0070df2f"), "E92000001", true, "MANCHESTER LIFESTYLE HOSPITAL", "TBS0657" },
                    { new Guid("05eab19b-68c9-43b1-9f38-c87bc1ddc0da"), "E92000001", true, "MANOR HOSPITAL [NUNEATON]", "TBS0659" },
                    { new Guid("2dc2d71a-b79f-4ef9-8b84-6bbf325bb9cf"), "E92000001", true, "MANOR PARK HOSPITAL", "TBS0660" },
                    { new Guid("42c12a49-b7ef-4539-bdce-ef314ae9e460"), "E92000001", true, "MANSFIELD COMMUNITY HOSPITAL", "TBS0661" },
                    { new Guid("c62075a8-2590-4623-a907-43d3109f6139"), "E92000001", true, "MARKET DRAYTON CLINIC", "TBS0662" },
                    { new Guid("93bf8408-fe62-4958-b031-0aabfcfe32e4"), "E92000001", true, "MARLOW HOSPITAL", "TBS0663" },
                    { new Guid("a219bc91-317d-4d34-9b1f-2aebd842ff67"), "E92000001", true, "MARY HEWETSON COTTAGE HOSPITAL (KESWICK)", "TBS0664" },
                    { new Guid("388884ed-1946-40a2-ab98-21c0cddeb2ad"), "E92000001", true, "MANOR HOSPITAL [BEDFORD]", "TBS0658" },
                    { new Guid("ccb52a7d-4fa7-40e9-a737-67122b16f974"), "E92000001", true, "MAUDSLEY HOSPITAL", "TBS0665" },
                    { new Guid("67396be1-9139-4766-b08f-05a4c4c1bcff"), "E92000001", true, "LOWER PRIORY HALL DAY HOSPITAL", "TBS0649" },
                    { new Guid("b284d100-3ee7-4f46-873d-3f75cf0f1540"), "E92000001", true, "LONGRIDGE COMMUNITY HOSPITAL", "TBS0647" },
                    { new Guid("b7a478b2-102b-4f92-870e-4ed979c162e4"), "E92000001", true, "LAMBETH HOSPITAL", "TBS0635" },
                    { new Guid("33468af8-31bf-40bf-a40c-a733879bf5e2"), "E92000001", true, "LANCASTER HOSPITAL", "TBS0636" },
                    { new Guid("a7311eba-644f-436f-ba95-dbdd124d999a"), "E92000001", true, "LAUNCESTON HOSPITAL", "TBS0637" },
                    { new Guid("4eecfcad-b8e1-4cbe-860b-6f7d42c06da0"), "E92000001", true, "LEATHERHEAD HOSPITAL", "TBS0638" },
                    { new Guid("1280ecc8-ff7d-473a-83d7-02e7fb2bafdb"), "E92000001", true, "LEDBURY COTTAGE HOSPITAL", "TBS0639" },
                    { new Guid("0df7bebb-e589-4bfb-a2bb-352ac1ccd2e1"), "E92000001", true, "LEICESTER FRITH HOSPITAL", "TBS0640" },
                    { new Guid("cb7c01c0-f60e-4071-b83f-3520279d6c4c"), "E92000001", true, "LONGTON HOSPITAL", "TBS0648" },
                    { new Guid("ced155c4-7eff-4c5e-8b38-1123d717d0bd"), "E92000001", true, "LEMINGTON HOSPITAL", "TBS0641" },
                    { new Guid("29bb0ca3-3329-4c44-a7d1-f5d0f77c3308"), "E92000001", true, "LEWES VICTORIA HOSPITAL", "TBS0642" },
                    { new Guid("7b0d2fa1-757d-4fed-940d-b697572e6315"), "E92000001", true, "LINCOLN HOSPITAL", "TBS0991" },
                    { new Guid("47d83ca2-4ece-4f3c-8771-c8c24564245e"), "E92000001", true, "LINGS BAR HOSPITAL", "TBS0643" },
                    { new Guid("0391ed46-0661-4cbe-924f-af0f5959ec19"), "E92000001", true, "LISKEARD COMMUNITY HOSPITAL", "TBS0644" },
                    { new Guid("13f42cb8-4430-487a-b995-f076dc4a26e0"), "E92000001", true, "LITTLE BROOK (APU) HOSPITAL", "TBS0645" },
                    { new Guid("c2769933-d4a2-4571-90a3-d624c0ab6c70"), "E92000001", true, "LITTLE COURT DAY HOSPITAL", "TBS0646" },
                    { new Guid("82321cab-3092-4fbf-8b4a-b82ba7ef341d"), "E92000001", true, "LEOMINSTER COMMUNITY HOSPITAL", "TBS0990" },
                    { new Guid("e883218f-e410-4256-aa98-47be80158122"), "E92000001", true, "KNUTSFORD & DISTRICT COMMUNITY HOSPITAL", "TBS0989" },
                    { new Guid("6fdcb0f8-4f98-41bf-a57e-e7f73f2f725d"), "E92000001", true, "MAYFAIR DAY HOSPITAL", "TBS0666" },
                    { new Guid("4c35309f-7645-48a3-9acd-b461687404e1"), "E92000001", true, "MEADOWBANK DAY HOSPITAL", "TBS0668" },
                    { new Guid("11d5a99a-ebbb-43aa-8643-07a3317cfea5"), "E92000001", true, "MOUNT GOULD HOSPITAL", "TBS0687" },
                    { new Guid("c6b597dd-0f08-47b1-95a9-2aba06df4db9"), "E92000001", true, "MOUNT HOSPITAL", "TBS0688" },
                    { new Guid("f26b852b-1da4-443b-a062-44696556e5f3"), "E92000001", true, "MOUNT STUART HOSPITAL", "TBS0689" },
                    { new Guid("65225bbb-bd5a-431c-9b0f-46e6349c03c2"), "E92000001", true, "NEVILL HOSPITAL", "TBS0690" },
                    { new Guid("1393efe2-9921-45d4-a384-c3ee9400c5b5"), "E92000001", true, "NEW EPSOM & EWELL COTTAGE HOSPITAL", "TBS0691" },
                    { new Guid("4a93fc07-d694-46be-bd34-fb31de211c57"), "E92000001", true, "NEW HALL HOSPITAL", "TBS0692" },
                    { new Guid("77ba7c6e-6aba-4575-8468-e8a522d5c478"), "E92000001", true, "MOSSLEY HILL HOSPITAL", "TBS0686" },
                    { new Guid("b0ff6cdd-9485-4e2f-b7cb-40c5161d631b"), "E92000001", true, "NEW VICTORIA HOSPITAL", "TBS0693" },
                    { new Guid("5ff90321-280c-4cc1-87d3-58b8012224e7"), "E92000001", true, "NEWMARKET HOSPITAL", "TBS0695" },
                    { new Guid("b3055ec0-0a3f-45d1-bba1-4bbd43aae087"), "E92000001", true, "NEWQUAY HOSPITAL", "TBS0696" },
                    { new Guid("0111f8e8-dabf-4d6a-9a00-c5878d8d9366"), "E92000001", true, "NEWTON ABBOT HOSPITAL", "TBS0697" },
                    { new Guid("3845346d-169d-47b0-903b-49cec8824b89"), "E92000001", true, "NEWTON COMMUNITY HOSPITAL", "TBS0992" },
                    { new Guid("4c68430e-9088-4351-814d-a17cae6e55fe"), "E92000001", true, "NORTH CAMBRIDGESHIRE HOSPITAL", "TBS0698" },
                    { new Guid("c3d08dd5-1216-4448-b62c-a0d2ca781294"), "E92000001", true, "NORTH WALSHAM COTTAGE HOSPITAL", "TBS0699" },
                    { new Guid("73119e11-1702-460c-b0f6-fb14ce31f0b0"), "E92000001", true, "NEWHAVEN HILLRISE DAY HOSPITAL", "TBS0694" },
                    { new Guid("462368bc-c8db-4369-83a3-6b8addcc6246"), "E92000001", true, "MCINDOE SURGICAL CENTRE", "TBS0667" },
                    { new Guid("f0ac5cc9-186e-4854-b103-98b515e91eac"), "E92000001", true, "MORPETH COTTAGE HOSPITAL", "TBS0685" },
                    { new Guid("7502a3d3-de66-4140-a6d6-960ef7c7917f"), "E92000001", true, "MOORGREEN HOSPITAL", "TBS0683" },
                    { new Guid("37c40954-7727-4726-804a-c7582f5c61d5"), "E92000001", true, "MELKSHAM COMMUNITY HOSPITAL", "TBS0669" },
                    { new Guid("6d3e4698-d919-4d98-bd8e-2e98d6473bc4"), "E92000001", true, "MIDDLESEX HOSPITAL", "TBS0670" },
                    { new Guid("679bc917-497f-49dd-8b91-ba5accf9be86"), "E92000001", true, "MIDHURST COMMUNITY HOSPITAL", "TBS0671" },
                    { new Guid("8d630c33-b77d-439a-b68a-a1bf4410c573"), "E92000001", true, "MILFORD HOSPITAL", "TBS0672" },
                    { new Guid("13b336fe-92de-474f-a896-2d859d233c3d"), "E92000001", true, "MILFORD ON SEA WAR MEMORIAL HOSPITAL", "TBS0673" },
                    { new Guid("11fe3038-e071-491a-a50b-53ce2933ce55"), "E92000001", true, "MILL VIEW HOSPITAL", "TBS0674" },
                    { new Guid("12ab2e21-d3b9-4120-b623-8f786467e5af"), "E92000001", true, "MORETON IN MARSH HOSPITAL", "TBS0684" },
                    { new Guid("dd42651a-effa-460a-b5bc-3401bf75a628"), "E92000001", true, "MILLOM HOSPITAL", "TBS0675" },
                    { new Guid("bd42b32e-820f-4961-a732-5e3bcb953f3f"), "E92000001", true, "MOLESEY HOSPITAL", "TBS0677" },
                    { new Guid("66a284f5-97a0-4962-876d-f33f0750a34b"), "E92000001", true, "MONKTON HALL HOSPITAL", "TBS0678" },
                    { new Guid("bc50c79a-5248-4b84-b4b7-027b2c59371d"), "E92000001", true, "MONKWEARMOUTH HOSPITAL", "TBS0679" },
                    { new Guid("55f81174-38cc-43b4-911a-1d1246db6f5c"), "E92000001", true, "MOORE COTTAGE HOSPITAL", "TBS0680" },
                    { new Guid("3df82031-1575-465b-a075-415e457bf7f5"), "E92000001", true, "MOORE HOUSE", "TBS0681" },
                    { new Guid("95563a29-52ea-4d9d-b22f-c644462cebf6"), "E92000001", true, "MOORFIELDS EYE HOSPITAL (CITY ROAD)", "TBS0682" },
                    { new Guid("6d393c1b-2af0-4be6-8450-fad7d03d4de1"), "E92000001", true, "MINEHEAD HOSPITAL", "TBS0676" },
                    { new Guid("cf398c9f-2fb9-4f22-a5a7-21687b670fb0"), "E92000001", true, "KINGTON COTTAGE HOSPITAL", "TBS0634" },
                    { new Guid("177e56f2-dd72-45e4-94c1-34244f00ede9"), "E92000001", true, "KING EDWARD VII HOSPITAL [MIDHURST]", "TBS0633" },
                    { new Guid("f025f44d-515c-4e9c-aabe-e914d94e6694"), "E92000001", true, "KILLINGBECK HOSPITAL", "TBS0632" },
                    { new Guid("2c18e17d-0e80-4342-8c85-4839f71c45ef"), "E92000001", true, "FROME VICTORIA HOSPITAL", "TBS0584" },
                    { new Guid("1cf07cb2-b6b4-44f0-9161-8e4349a7fbee"), "E92000001", true, "FRYATT HOSPITAL AND MAYFLOWER MEDICAL CENTRE", "TBS0585" },
                    { new Guid("51e00361-b228-4e21-8efd-06edd9cbb42c"), "E92000001", true, "FULWOOD HALL HOSPITAL", "TBS0586" },
                    { new Guid("c3bf9ab5-a70f-425d-9c61-4d270ae5a80c"), "E92000001", true, "GARDEN HOSPITAL", "TBS0587" },
                    { new Guid("49c09825-1bec-45d2-a2e9-d6219c5b02eb"), "E92000001", true, "GENERAL LYING-IN HOSPITAL [LONDON]", "TBS0588" },
                    { new Guid("a74eda3a-bf03-49f6-a1bb-4ba277d6336b"), "E92000001", true, "GLENSIDE HOSPITAL", "TBS0589" },
                    { new Guid("f025fbe2-464a-4f53-aa78-6e02c5c116b3"), "E92000001", true, "FRIARY HOSPITAL", "TBS0583" },
                    { new Guid("3a1e6946-5300-4de1-93af-de7452f7a809"), "E92000001", true, "GLOUCESTER HOUSE/ DORIAN DAY HOSPITAL", "TBS0590" },
                    { new Guid("698f063b-feaa-4921-b3b1-db97668615d3"), "E92000001", true, "GORING HALL HOSPITAL", "TBS0592" },
                    { new Guid("ea38954b-b569-46b4-ac8f-e36a236eb4d2"), "E92000001", true, "GORSE HILL HOSPITAL", "TBS0593" },
                    { new Guid("511fedce-81a6-44da-ab0a-7950ae3762fc"), "E92000001", true, "GOSCOTE HOSPITAL", "TBS0594" },
                    { new Guid("d1de4314-6221-4270-a11f-4c230da0c71d"), "E92000001", true, "GOSPORT WAR MEMORIAL HOSPITAL", "TBS0595" },
                    { new Guid("9a60d410-2ce3-4611-82cf-b4a0196ff5e1"), "E92000001", true, "GREATER MANCHESTER WEST MENTAL HEALTH NHS TRUST", "TBS0986" },
                    { new Guid("c717489d-8ca6-4da9-91fa-f22bc9cd7427"), "E92000001", true, "GROVE CONVALESCENT HOSPITAL", "TBS0596" },
                    { new Guid("bfe242b1-00bb-4e30-a0a8-9614bbd2fd52"), "E92000001", true, "GORDON HOSPITAL", "TBS0591" },
                    { new Guid("d586409a-1bf3-4b39-8d0e-7ca0537d1cef"), "E92000001", true, "GROVE HOSPITAL", "TBS0597" },
                    { new Guid("96fa60e4-55df-43a9-bb6e-86e303cd2d67"), "E92000001", true, "FRANKLYN COMMUNITY HOSPITAL", "TBS0582" },
                    { new Guid("6ac374a6-4151-40a4-943e-4e9f42d30b36"), "E92000001", true, "FORDINGBRIDGE HOSPITAL", "TBS0580" },
                    { new Guid("7ade1b25-109f-4b96-8eed-7fc6c7b32d6b"), "E92000001", true, "EUXTON HALL HOSPITAL", "TBS0566" },
                    { new Guid("2c136d6d-d644-4cf2-a995-b2f9e5218a4d"), "E92000001", true, "EVESHAM COMMUNITY HOSPITAL", "TBS0567" },
                    { new Guid("dc90bc15-6ebe-4d24-b9d1-d1d2b2ace8eb"), "E92000001", true, "EXMOUTH HOSPITAL", "TBS0568" },
                    { new Guid("e59daa5f-b8f2-441d-b116-9a5eb1ba05cd"), "E92000001", true, "FAIRFORD HOSPITAL", "TBS0569" },
                    { new Guid("101c3e99-befb-48c9-bcd1-7c716197fa11"), "E92000001", true, "FALMOUTH HOSPITAL", "TBS0570" },
                    { new Guid("4f27a05e-b05b-4a76-8ae7-358d947c59c9"), "E92000001", true, "FARNHAM HOSPITAL", "TBS0571" },
                    { new Guid("73a5df50-ee94-4dd2-b0b0-e00bef38fff9"), "E92000001", true, "FOSCOTE HOSPITAL", "TBS0581" },
                    { new Guid("6dc8b7ba-3315-49ae-aa00-9f7649998706"), "E92000001", true, "FARNHAM ROAD HOSPITAL", "TBS0572" },
                    { new Guid("a147d206-4a86-4665-bc81-21c651334073"), "E92000001", true, "FELIXSTOWE HOSPITAL", "TBS0574" },
                    { new Guid("cc2a4a36-92dd-4ac1-97ee-9648940a0dea"), "E92000001", true, "FENWICK HOSPITAL (PERIPHERAL CLINIC)", "TBS0575" },
                    { new Guid("a7ecceeb-ea05-42e8-a6ef-0119e309c5a6"), "E92000001", true, "FIELDHEAD HOSPITAL", "TBS0576" },
                    { new Guid("cf00498b-dbb6-48f1-83ec-aae4572daad4"), "E92000001", true, "FINCHLEY MEMORIAL HOSPITAL", "TBS0577" },
                    { new Guid("f9454382-9fbd-4524-8b65-04c1b449469c"), "E92000001", true, "FLEET HOSPITAL", "TBS0578" },
                    { new Guid("1ee2b39a-428f-44c7-b4bb-000649636591"), "E92000001", true, "FLEETWOOD HOSPITAL", "TBS0579" },
                    { new Guid("7a96bb0c-38d0-4b16-bf21-4f49df83d86f"), "E92000001", true, "FAVERSHAM COTTAGE HOSPITAL", "TBS0573" },
                    { new Guid("1176df5e-8ff1-414f-abb7-0db747120689"), "E92000001", true, "GULSON HOSPITAL", "TBS0598" },
                    { new Guid("0d43f157-c95d-49f9-9c8a-33b8f1321c37"), "E92000001", true, "HALSTEAD HOSPITAL", "TBS0599" },
                    { new Guid("d6d3ad1b-1470-4840-bbd4-1127d47295cd"), "E92000001", true, "HALTWHISTLE WAR MEMORIAL HOSPITAL", "TBS0600" },
                    { new Guid("3c55dd08-0747-470d-8a17-ba1deb200662"), "E92000001", true, "HOLMEVALLEY MEMORIAL HOSPITAL", "TBS0620" },
                    { new Guid("21049817-2eed-40a9-bcca-0e3b5f6185c1"), "E92000001", true, "HOLSWORTHY HOSPITAL", "TBS0621" },
                    { new Guid("1a69d10b-f4c7-4061-abed-58f6131d45f7"), "E92000001", true, "HOMELANDS HOSPITAL", "TBS0622" },
                    { new Guid("01037c28-d804-437c-a7d8-7240bac7e32c"), "E92000001", true, "HOMEOPATHIC HOSPITAL", "TBS0623" },
                    { new Guid("584eceab-da04-47c8-82dd-4ab23a1f6d21"), "E92000001", true, "HORN HALL HOSPITAL", "TBS0624" },
                    { new Guid("0463d983-c9ae-478c-8f7d-2593cc1d7bef"), "E92000001", true, "HORNSEA COTTAGE HOSPITAL", "TBS0625" },
                    { new Guid("48614141-0cee-4337-803c-9ae4e0b12ae3"), "E92000001", true, "HOLLINS PARK HOSPITAL GERIATRIC DAYCARE", "TBS0619" },
                    { new Guid("8c5315c5-afbc-4a64-bf67-d8d0ffbe8b76"), "E92000001", true, "HRH PRINCESS CHRISTIAN'S HOSPITAL", "TBS0626" },
                    { new Guid("120555e5-8b92-4698-9377-a74be41ac936"), "E92000001", true, "HUNTERS MOOR HOSPITAL", "TBS0628" },
                    { new Guid("02f37746-53f5-4651-94fc-bd3607e07709"), "E92000001", true, "HYDE HOSPITAL", "TBS0629" },
                    { new Guid("fb02a43a-455a-4e3c-a16e-3799bfcf6312"), "E92000001", true, "HYTHE HOSPITAL (PERIPHERAL CLINIC)", "TBS0630" },
                    { new Guid("9dc201fb-9fe3-4960-92d7-8b8db7f3a0ec"), "E92000001", true, "JOHN COUPLAND HOSPITAL", "TBS0987" },
                    { new Guid("b7a95499-6115-4c97-ba9f-8f2fbef86169"), "E92000001", true, "JOHNSON HOSPITAL", "TBS0988" },
                    { new Guid("00caa2cd-0224-4b6d-beeb-caedc8a6dd2a"), "E92000001", true, "KEYNSHAM HOSPITAL", "TBS0631" },
                    { new Guid("bc13b36e-d30d-4795-aa0e-d0a69894534c"), "E92000001", true, "HSH BROADMOOR HOSPITAL", "TBS0627" },
                    { new Guid("fe09a91e-aa63-47b0-9464-b00d32ebfe54"), "E92000001", true, "HOLBEACH HOSPITAL", "TBS0618" },
                    { new Guid("9cadcafd-3526-4afc-8a4b-d1f1185c6e60"), "E92000001", true, "HIGHWOOD HOSPITAL", "TBS0617" },
                    { new Guid("330041e1-4952-4bba-ab0e-71a36cd7b69b"), "E92000001", true, "HIGHBURY HOSPITAL", "TBS0616" },
                    { new Guid("33c3bf94-1cfc-4778-a076-3a91e2efaf75"), "E92000001", true, "HAM GREEN HOSPITAL", "TBS0601" },
                    { new Guid("70c91a44-6abb-4866-841f-44550c2cb8cc"), "E92000001", true, "HARBOUR HOSPITAL", "TBS0602" },
                    { new Guid("9a14b7bb-89ad-440b-9fcc-cea488a9deab"), "E92000001", true, "HARPENDEN MEMORIAL HOSPITAL", "TBS0603" },
                    { new Guid("9df6af00-f484-4f6c-8aa6-84acc47c64aa"), "E92000001", true, "HARPERBURY HOSPITAL", "TBS0604" },
                    { new Guid("f11f97b1-3e83-4c7a-a5ef-06c1846ce98e"), "E92000001", true, "HARPLANDS HOSPITAL", "TBS0605" },
                    { new Guid("b13452a4-becf-4e6c-8edf-2e6995b4c5a5"), "E92000001", true, "HARTISMERE HOSPITAL", "TBS0606" },
                    { new Guid("3d8fb352-645a-4a16-9ab2-db0ed891c3bb"), "E92000001", true, "HASLEMERE HOSPITAL", "TBS0607" },
                    { new Guid("f261f4c6-1812-48a1-851f-90e88cac5ddc"), "E92000001", true, "HAVANT WAR MEMORIAL HOSPITAL", "TBS0608" },
                    { new Guid("de169d34-4dc6-4a05-87eb-20901416dafb"), "E92000001", true, "HAWKHURST HOSPITAL", "TBS0609" },
                    { new Guid("a53aa091-d475-4f4e-bc7c-20361c476a29"), "E92000001", true, "HAYWOOD HOSPITAL", "TBS0610" },
                    { new Guid("6f8f4ef3-1f6f-4706-8eec-461772615796"), "E92000001", true, "HEAVITREE HOSPITAL", "TBS0611" },
                    { new Guid("79b5d236-2766-4f92-9ce3-826ef6bc319d"), "E92000001", true, "HELSTON HOSPITAL", "TBS0612" },
                    { new Guid("84eebf2c-d40b-4e57-b392-8085eaf6fbdf"), "E92000001", true, "HERBERT HOSPITAL", "TBS0613" },
                    { new Guid("1859ff13-82f8-4851-be86-8239fc495eed"), "E92000001", true, "HERTFORD COUNTY HOSPITAL", "TBS0614" },
                    { new Guid("f143cc8c-b3bd-4225-a0ba-2ecfcd46144c"), "E92000001", true, "HERTS AND ESSEX HOSPITAL", "TBS0615" },
                    { new Guid("34cc991a-c066-4dc9-a272-dadbddd46d0f"), "E92000001", true, "YEATMAN HOSPITAL", "TBS0981" },
                    { new Guid("107ca001-2370-4a82-9cdf-5591499dcbf0"), "E92000001", true, "ZACHARY MERTON HOSPITAL", "TBS0982" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("00caa2cd-0224-4b6d-beeb-caedc8a6dd2a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("01037c28-d804-437c-a7d8-7240bac7e32c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0111f8e8-dabf-4d6a-9a00-c5878d8d9366"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("023f1000-b4d1-4887-b601-333a87bb6514"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("02f37746-53f5-4651-94fc-bd3607e07709"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0370a142-f5ed-4c1e-991c-5d92b2aacb40"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0391ed46-0661-4cbe-924f-af0f5959ec19"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("04248670-4d1f-4026-bf22-e913208e672e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("042fa0e4-791a-47a0-9f5f-2c1348954954"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0463d983-c9ae-478c-8f7d-2593cc1d7bef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("04dd6731-97a7-4fad-bfde-d95c98cd04c5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("053ae50b-3b39-44b7-89ec-1bce415bd846"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("05eab19b-68c9-43b1-9f38-c87bc1ddc0da"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("06749b63-ae06-4cc6-9d66-9dd5c4a0245e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("07c51540-3fb0-44d6-8c40-c98a4ff59ae2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("097ea09c-71a4-4178-bf54-c2858ad9d493"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0a77a26e-c6cf-4f2d-8501-8d94ea136d1d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0a8ab02a-c64b-459e-8ff8-1f96bb8a0159"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0c144e14-ed87-4980-b962-6480bb945afd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0d43f157-c95d-49f9-9c8a-33b8f1321c37"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0dcae460-f8cc-44af-b6c1-8b86b0a725d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0df7bebb-e589-4bfb-a2bb-352ac1ccd2e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("0e76ada3-8ccd-4426-a6d0-4b08663c38a5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("101c3e99-befb-48c9-bcd1-7c716197fa11"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("107ca001-2370-4a82-9cdf-5591499dcbf0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11081272-a36b-459d-9e6b-7e8c486fdc9f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1176df5e-8ff1-414f-abb7-0db747120689"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11b304c6-f0bc-498f-925b-1fc8e1c7031a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11d5a99a-ebbb-43aa-8643-07a3317cfea5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11e3674c-17d3-4c4e-8f2c-a4af70dd6fb0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("11fe3038-e071-491a-a50b-53ce2933ce55"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1200824a-cce6-491d-bd93-33e44f0b383b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("120555e5-8b92-4698-9377-a74be41ac936"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1219b1c8-850c-483f-bf3b-16dc9090dc7b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("121c9756-16e7-4186-a1d4-e36b51b57dfb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("12759488-b4db-4f7c-ab2e-f13bd8c6e73d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1280ecc8-ff7d-473a-83d7-02e7fb2bafdb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("12ab2e21-d3b9-4120-b623-8f786467e5af"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("131ccafa-9147-4bd3-93f4-157c70b56b4b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1387c5d9-b653-43bd-93fc-20ce75edd420"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1393efe2-9921-45d4-a384-c3ee9400c5b5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13958bea-c1bc-4ccf-ad03-e9bbfa016502"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13b336fe-92de-474f-a896-2d859d233c3d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13b6c143-587d-46f8-ba7b-5341482066f4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("13f42cb8-4430-487a-b995-f076dc4a26e0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("14476a1d-6db0-49ae-bf4d-c0d95b51af21"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("148130af-457a-4509-946d-f6e6736f22ff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("14f099af-044d-4a30-ae8d-6aef04dbd1a1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("15e5950d-fc36-439d-aab0-ca520e0b617c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("16908b7a-58f8-4e17-a8e8-91888d86f52f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("16aba4bf-274c-465e-9bfd-ff3309266e97"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("171806a0-98a7-4ae6-bac2-ffcb4f3fc092"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("172b8133-4b56-4858-982a-a2b8cb4cd46d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("17340cde-67c7-4203-9957-afa0c3275895"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("177e56f2-dd72-45e4-94c1-34244f00ede9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("179b430d-5d8c-4f21-9ba8-c6a652df0d62"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("17f93ffc-01e5-48e1-b1fd-10fc7308f9ec"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1859ff13-82f8-4851-be86-8239fc495eed"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1895e8b7-ab2c-4307-adf3-f36a119a32a4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("18b079d8-ba76-4145-930b-547d4c3b623f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("18b6e8b8-c327-4f37-9143-c215bad7faf3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("19835e7e-2d97-4ee5-833e-62a603ba2685"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1a2ad18f-94ef-4c63-ab09-40ac192343c7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1a498852-ef37-4a94-9a87-44fba3869b70"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1a69d10b-f4c7-4061-abed-58f6131d45f7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1b917fe6-3fb7-4fba-b5f5-744c69f9ca60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1bdbc2b9-1971-4af8-ac2a-d3a8f06b8399"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1be6ea32-ab5d-4efe-ac25-371d7b2525bd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1c39b75b-ba22-4574-afc0-459c0009d4b7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ca2bb07-2fb7-4e0c-a853-0568d7452ba0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1cf07cb2-b6b4-44f0-9161-8e4349a7fbee"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1cfd833f-76ca-41e5-b151-65045df09358"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1d6e8331-f3c2-4b5d-8415-e98dc82f72de"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1e2e25df-bd19-4647-82f0-bd3b3af5ec19"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ebcb983-55d2-4365-ae0f-c91d1d19ef86"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ee2b39a-428f-44c7-b4bb-000649636591"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1ff7f5f2-1f35-47f0-8e3f-891eb4fdd2bd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2082a678-403f-4969-8b07-557ce2240409"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("20f1a936-225b-4b47-9ecd-cedc4fb2fd45"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("21049817-2eed-40a9-bcca-0e3b5f6185c1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("214ec28c-2a69-4940-89ef-136bc462dfbe"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("22e73828-676d-4c3e-bc25-092cb00c0ae0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2332b4f9-b6c0-42a5-86d6-eca9837d40a7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("23aa321d-33d9-4b4f-ac38-f585a004b1d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("24766cb2-0f78-4c8f-a188-57f1e3fe9a30"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("251ca4b4-7699-4927-a36c-f2d53f737d1f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2599dd55-6274-41e1-ad21-3dbc0d2b8349"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("25bf770e-faa6-4e5e-b412-a125ddec94ed"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("265ba139-52f9-4207-b10a-feedb861cf85"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("266014b2-0bc5-4daf-9bff-219f1c77db68"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("26b15340-f812-4993-87d8-1a6e32c8115c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("26b4b173-8990-41c6-8099-405d26f158d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("26c2e669-97e7-4e78-97e2-b87af2729ea7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("27988285-43f2-41d6-a40b-a98fe588da6e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("27bb6d5a-ffb2-448c-8583-19dd3d471c02"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("28dcb0dc-31d4-4b7a-ad44-3e27cb37016c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("292d7a2b-1b87-4644-ad43-83f1301a0c94"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("29bb0ca3-3329-4c44-a7d1-f5d0f77c3308"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("29d4ded5-bcfb-4cfd-b6da-1f3ef98d3a64"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("29f693f4-4738-4163-a6a5-71005d9601bb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a1aede9-90b4-4967-84c7-e2788fde1eec"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a4164c7-4487-4e33-acb1-7cd51f7ad158"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a6fa966-d70f-48f0-a72a-782827ee1974"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2a9c7e60-af14-4929-8233-79deb17855bd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2b0c8ab0-4909-4c9c-9441-fc510b936d41"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2b5f6287-4b9f-4eb6-97c3-abbbfbfeb84e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2b73ae84-1c40-4098-9679-86f512999682"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c136d6d-d644-4cf2-a995-b2f9e5218a4d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c18e17d-0e80-4342-8c85-4839f71c45ef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c216f57-6c1a-4e50-811a-5f8a5b8568b3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c222cd0-880d-4b83-9e61-31b820c01626"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2c8ba49a-64a7-43fa-81eb-a072b2a0253b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2dc2d71a-b79f-4ef9-8b84-6bbf325bb9cf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2dfe6f2d-1a1e-45ca-82aa-cd19e85fdaee"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2e04b26e-1e81-4022-8d4e-f3817c3fbe47"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2e32407c-b568-447f-8c4a-052dd3a1fd67"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2e96d1a5-91d4-4f82-9b37-e090dbe091be"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("303e9e42-6d29-43a2-858c-e67554ba4ed9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3074f823-eb92-47b5-b434-75b860c5d7cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("30b7756e-486a-40c7-a24d-ff5ca4860db9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("31140ae9-27d7-4860-833b-673dab14fd0e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("312b9df1-b85b-4b11-94e8-4b2034125b3f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("31b6677d-f5df-4101-950d-cd47878edf55"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("31c14107-8e42-43d6-8dfe-bbe5b19f1d4a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("323f047e-5341-424a-886c-0324ab669fe7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("330041e1-4952-4bba-ab0e-71a36cd7b69b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33464912-e5b1-4998-afca-083c3ae65a80"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33468af8-31bf-40bf-a40c-a733879bf5e2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("335fe746-12f2-4c42-9f28-7f21a3535ad4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33c3bf94-1cfc-4778-a076-3a91e2efaf75"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("33ee2791-894f-45b2-880c-e9b83854fd07"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("34a57071-c138-4f11-b5e8-15bbc4b5462d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("34cc991a-c066-4dc9-a272-dadbddd46d0f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("35d8d223-8e22-4601-9abf-32b86bb8bc0c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("36910905-2275-4316-beb5-241396e8b893"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("37187ac2-5da3-46a1-b6b1-2dff7c8c9720"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("379d9d0f-a629-4d21-bdbb-1702f2960ee8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("37c40954-7727-4726-804a-c7582f5c61d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("383386f0-421f-46d6-b037-b73b44be95ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3845346d-169d-47b0-903b-49cec8824b89"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3846f91b-107f-430a-badf-49fdf130079a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3871aa2d-b501-4e53-8d6e-346beebe9167"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("38872531-c7ec-4f84-b3c2-3d3f0c923c18"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("388884ed-1946-40a2-ab98-21c0cddeb2ad"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("391ed44b-76fb-4cd1-8eb8-00ad07336412"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("391fbb68-4531-4f6c-9e8d-b79b8ca93608"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("39530369-9873-483e-a218-114c3bcc6240"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("397448bf-759f-4f96-bf78-a885710a2450"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("39ef6af3-6c00-4d52-83fe-39f398809e46"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3a1e6946-5300-4de1-93af-de7452f7a809"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3a2b45af-6c2c-4420-9b93-c54476743324"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3b110ed9-3faf-458e-a188-e6c6d719707c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3b194a7c-455b-4dbd-a9c7-95acc378e68e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3c55dd08-0747-470d-8a17-ba1deb200662"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3d8fb352-645a-4a16-9ab2-db0ed891c3bb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3dbcf14f-400a-46b0-b179-1aa84e656724"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3df82031-1575-465b-a075-415e457bf7f5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3e5952e3-4d49-4c01-8ac4-e91928af25a3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3ed63450-486c-4ade-a188-07f6c9e8139d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3f1725c7-758a-42a4-ab5d-21ee6ad2c8d5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3fa1136d-a326-49e0-907f-8e12db7f8429"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("3fe6c7bf-c0bd-4b9f-87f5-1bac12969a91"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("40bcdeef-7d37-4632-9893-70f454a50045"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("40c7827b-9556-492f-af7b-f2c4a5a3f1f2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("41230103-eafc-4e94-8a18-00a4233ae90d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("42c12a49-b7ef-4539-bdce-ef314ae9e460"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4340fe30-9786-4feb-9d90-e1c82f340113"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4424dc08-b730-4a64-b252-0b138dd52699"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4485e161-a44c-43fd-b43f-a038f3beb012"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4513e509-0e43-4c2f-b4b4-9abd4d81a856"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("458809a5-5ae8-4d9d-8af3-d4249d14a5de"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("459820f1-9c30-4998-a207-9dbada17108c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("462368bc-c8db-4369-83a3-6b8addcc6246"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4745febe-293f-40ec-bc10-8490a4b245cd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("476fec49-f090-42f4-9c7c-058e1c10e226"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("47d83ca2-4ece-4f3c-8771-c8c24564245e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4836a4e5-d0a3-43de-b8e7-d4bfe81b7e9c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("485bf2da-6079-4160-bd84-8eb30559c63c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("48614141-0cee-4337-803c-9ae4e0b12ae3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("48b7a7b0-ed79-45af-92cf-25445c376102"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("48fc350e-6fd5-4087-9b3d-a5b2c43c2b45"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4970ea1f-b07d-4290-98c7-1cbfc055829e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4987e982-7774-42ac-b22f-a24bdf5e2c22"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("49ae657f-2a66-4674-8aba-060bc6388a08"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("49c09825-1bec-45d2-a2e9-d6219c5b02eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4a1177f9-6a35-43e9-b106-c309bd5313dd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4a8aa3dc-1da9-42cd-b9ae-4e882255d86a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4a93fc07-d694-46be-bd34-fb31de211c57"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4b61f76e-b9a2-4aef-a403-95a0e922b603"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4b6fa7ab-b480-4651-8831-abc0f78755af"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4c35309f-7645-48a3-9acd-b461687404e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4c68430e-9088-4351-814d-a17cae6e55fe"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4d5c6fdb-dc53-44d3-8800-6a6babc9ea35"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4de9449e-65f5-40c0-9c32-953d189f20df"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4e9c7a1d-a42d-414a-9d14-4e2986c939f0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4eecfcad-b8e1-4cbe-860b-6f7d42c06da0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4f24dd69-5bc8-4f1c-99f5-2260817643a6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4f27a05e-b05b-4a76-8ae7-358d947c59c9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("4fcd15ef-14c7-454b-aae4-4ecdb49f9d6e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("509797da-05a6-4c9e-8168-d46a7f22d867"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("511fedce-81a6-44da-ab0a-7950ae3762fc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("51e00361-b228-4e21-8efd-06edd9cbb42c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("51f4291e-dddd-497d-bd04-158763e1a131"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("52399c6d-9d4a-4286-963f-7ec91adf946e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("533f62df-234b-41a9-8735-27b0849fa018"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("53737ff9-2b2e-4800-9851-5967dacd26d9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("54b0bd76-51e1-484a-8ad8-deeaa419d564"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5524785c-7823-41df-9341-6b4a65aa9c19"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("55f81174-38cc-43b4-911a-1d1246db6f5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5740e112-41b7-4b5a-aec0-1d15fdb6d997"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("57725cc0-5c4d-4ac9-8de6-5355cd8abe66"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5786ff70-02fe-4d20-bad7-affd201fe419"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("57e673cc-ed70-442f-bd96-0f261fc88c8c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("584eceab-da04-47c8-82dd-4ab23a1f6d21"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5ab01d61-cf4b-46ec-8792-44632e8a7e4c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5b41832a-3656-4a4c-b3a7-e6d3d67d76d8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5c0068e6-fa8e-43de-ad7e-6d2b7c497e10"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5c5cbc05-7e6a-4b10-add0-7f7638ec1918"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5d3afb26-c8a6-44f6-a00a-f50d23fb220e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5d75da70-d99e-46d6-a066-97347f6ee5c4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5f17d1c6-6964-447a-aaa5-791e8895b8b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("5ff90321-280c-4cc1-87d3-58b8012224e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6050e0bd-f829-403f-83f2-6ff2865b1f2d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("60593be6-cc9f-4a86-bbc0-f65da4b79b51"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("609bbae6-b46b-4e9f-bcc4-44bd5b01599a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("60d1768d-6d86-4a22-adc9-220b6a075308"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("60e405a2-4d47-4a49-a739-dbedfb7bf9b3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("61ed6301-8b4a-4ed0-93ba-2fa07b5c8796"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("629bfd57-7c1d-438e-9cdf-c1524f885090"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("62be61a4-7d43-415b-baae-92e83e521140"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("631c5c01-a844-49eb-8cdf-7555416cab1f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6344cd67-0f25-4288-9026-6af917df5790"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6385964d-19af-4fb0-ac9f-122a64305752"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("64150436-2aac-45af-b6c0-5f948dc3cbf3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("64a797c5-6852-4095-97ae-3bd45beb357f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("65225bbb-bd5a-431c-9b0f-46e6349c03c2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("656a1449-4e9b-4dee-bf0f-8c6d0070df2f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("66a284f5-97a0-4962-876d-f33f0750a34b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("671be757-50e5-4636-912e-707241af5149"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("67320253-12f2-4c75-ba77-a916db405cb3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("67396be1-9139-4766-b08f-05a4c4c1bcff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("67956308-b9df-4107-b50f-3f6c911005ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("679bc917-497f-49dd-8b91-ba5accf9be86"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("68806afc-e4a9-472b-8899-ba0426b1ebe5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("695d7449-a7bf-46dd-9e7e-6d49f05c24d1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("698f063b-feaa-4921-b3b1-db97668615d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("69bddd08-ed80-4885-869c-4714dc02ce06"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a0db87b-8a6e-48ab-964a-2e785cc3d6d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a4403f4-4e9d-44d5-b78e-b330547bb781"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a4c0d9b-fcbe-44b5-900f-558b10ed9ecc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a7f7bd2-5667-4cf8-a857-e8ae47871898"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a939b10-83d6-4ecd-97df-c3f732f3f77f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6a9e42da-c4ba-48f9-a6ac-acc852027a87"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6ac374a6-4151-40a4-943e-4e9f42d30b36"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6d393c1b-2af0-4be6-8450-fad7d03d4de1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6d3e4698-d919-4d98-bd8e-2e98d6473bc4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6db4a299-9431-4e71-a0d9-c97b186ce043"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6dc8b7ba-3315-49ae-aa00-9f7649998706"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6e03ad18-2623-42cf-a39a-1a78dcef8714"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6f637c51-b8aa-4fff-a796-cf69bdaa49f7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6f8f4ef3-1f6f-4706-8eec-461772615796"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6fdcb0f8-4f98-41bf-a57e-e7f73f2f725d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6fe27200-3464-48af-99f3-745c6a868089"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("70693b46-61f4-4321-a896-a9913d0aa600"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("70c91a44-6abb-4866-841f-44550c2cb8cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("70f2cca0-2289-4482-9ebf-0be8e031203f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("71259dbf-b9b8-46ed-a7bd-9c256bdf848b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7306f844-2521-48d0-a765-2c1e45621d88"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("73119e11-1702-460c-b0f6-fb14ce31f0b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("733fd6d6-be41-42ed-b105-68acd32b678f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("73a5df50-ee94-4dd2-b0b0-e00bef38fff9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7502a3d3-de66-4140-a6d6-960ef7c7917f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("75a8d82b-93a2-411f-b082-b26fde40217d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("768011c9-e0c4-4b1b-84a3-317c4bf2f402"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("768cfc30-fc07-420f-86ce-d6f91501d253"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("778a2fae-8eb6-440b-b2c6-056b637b94e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("77b00623-0efd-4e7e-9cfd-b6a017cc9738"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("77ba7c6e-6aba-4575-8468-e8a522d5c478"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7877b857-c94d-47c1-9636-e0ef6005ef89"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("79236fd5-e97d-45e7-9a5b-5ee9b623c6d3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("795bb9c1-8d85-4b38-ad40-d77344139fc1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("79b5d236-2766-4f92-9ce3-826ef6bc319d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7a96bb0c-38d0-4b16-bf21-4f49df83d86f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7aaec4ba-fee4-4db5-8498-df3987bb620f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7ade1b25-109f-4b96-8eed-7fc6c7b32d6b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7b0d2fa1-757d-4fed-940d-b697572e6315"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7c1c7f89-d326-42ec-8edd-d67cd3e52488"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7c5a090e-b20b-4dd0-938d-03aabc914eb2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7c6e024a-27a7-4763-b1a7-30f442aea7bf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7e1071f0-410c-4f32-8dd0-3763d23605e3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7e12c815-5491-4cf9-aa03-5f4528aed0e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7eda8526-ce1a-4bba-98d3-32e6e3ca816e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("80018752-3ab1-4285-b26f-34655f5c4ed7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("805559be-9862-4194-9a37-9a9d93b73be6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("80608b56-dc0e-4205-ad07-5fbbc878d679"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("80948edb-30fa-4d22-8f2c-c9bcc886c6ee"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("80ffe3cc-baba-4785-963c-81b6a30a217d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("818340c5-52a3-44c5-be80-957033a3dcda"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8218a639-27b1-4a8c-8f44-3ca722294c4f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("82321cab-3092-4fbf-8b4a-b82ba7ef341d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("84576348-2dca-4487-b0a9-46268f222fa6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("84eebf2c-d40b-4e57-b392-8085eaf6fbdf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("85364a25-a67d-4194-83f0-d48e848b3fac"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("86101239-4962-40a4-90f3-48d6fd2bf8e2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("868dbe9d-8a26-4e06-ae43-9abeb6202aec"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("87ac9a14-5333-4de8-bac9-cbc1d5bf86a6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("87ec205d-9067-4c55-b3b2-0595705af675"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("87f31e3d-d28f-4f61-8b8c-17ffef38e149"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("88b3881c-9401-480c-8f95-53fa0ad345c2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("89a79fd3-e59f-4255-9e7a-2b536dbb5075"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8a702ecd-96fd-457f-a931-4b367f12277a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8a7d861d-5086-4fba-a813-20fb895c8412"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8b3b8368-fcf4-4612-b78a-e7a00e7fa826"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8bac3ff5-fd44-4989-b4e7-0b1846686eb3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8c00f2ef-4a63-4f7c-a264-552e5ea5c1f2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8c5315c5-afbc-4a64-bf67-d8d0ffbe8b76"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8d01039e-986e-4e26-ad3e-2e30729da071"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8d630c33-b77d-439a-b68a-a1bf4410c573"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8e2e428a-341c-4f76-9633-124d5ca342b5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8fa5e547-b7c7-45cb-8912-8609d6c16f1d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("904ac274-f26b-4b8a-9fe3-8ab27cca3e9f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("90cd195c-ca0b-41fe-a04b-70d2c90c15b7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("910d3ae1-0c2c-4ed6-baac-0e1e69260969"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("919633a7-e62d-490f-8e4b-84339da0ab9b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("91c4fcb5-6ca9-468f-9085-55d267414263"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("93bf8408-fe62-4958-b031-0aabfcfe32e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("93ccb810-7dfd-44a5-afe4-69c9d9c6d314"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("95563a29-52ea-4d9d-b22f-c644462cebf6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9583590e-1925-4f46-9025-aeb6d5a04b07"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9623526c-0c4b-4ff2-a99d-4b8ed508a0ce"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9670dd65-0d74-423c-ad51-cd1d4c638d03"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("96ec72d1-fdcf-4988-b73a-e3fc5a83f49d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("96fa60e4-55df-43a9-bb6e-86e303cd2d67"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("97e4ac81-920c-4877-9151-96b8aef40c36"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9857ff70-d992-4e8d-b97d-d174b70dfb74"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("98f82c09-d243-4c39-8260-4874c71a0433"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a08a374-5112-4386-9ea1-100819754d8b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a14b7bb-89ad-440b-9fcc-cea488a9deab"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a45c03f-ab96-45a7-8f11-36c4bb210f60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a60d410-2ce3-4611-82cf-b4a0196ff5e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9a6d0dfa-73f2-4df5-be61-95e4834cf564"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9ba3ecfd-157a-418d-9fe4-5fc83add48b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9cadcafd-3526-4afc-8a4b-d1f1185c6e60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9d01552f-d274-4cbc-a4a6-a15e4d34a72c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9dbbac50-5607-4e9b-b21c-a1fafdd1c36d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9dc201fb-9fe3-4960-92d7-8b8db7f3a0ec"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9dd22d58-44dc-4a73-9d52-e36cf488e570"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9de16b29-e316-4d88-ad2a-e47a0b413a3d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9df0c003-65cb-4c9b-a667-8eba40d5fe62"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9df6af00-f484-4f6c-8aa6-84acc47c64aa"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9e12885c-7511-4266-8eb9-19175f116167"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9e2bd90d-dd34-4355-af2a-2917065629f0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9e4fd059-9f62-4e75-b049-33bf60d7e594"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9f8eb5c4-0fc0-4c84-bfdb-eed9b1d9cc94"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("9fa43296-0946-4e57-bb3c-1e599b6d611b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a13d2e29-6f40-488f-8fc4-ba84101cfe0f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a147d206-4a86-4665-bc81-21c651334073"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a219bc91-317d-4d34-9b1f-2aebd842ff67"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a22dda9e-0c7d-475f-ba10-6b76d549f920"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a25a8a6c-4223-437e-a1a4-96894227549c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a2c4eace-1bef-4773-bdbc-d6fad216867b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a4da4b5d-3907-4962-88c7-04b387471131"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a4ddff83-2c02-4377-a034-0304e5e3c374"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a4eaac12-239b-4e65-ac79-e7858080a94e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a53921b3-05d5-485c-9dcf-9cd7371d5a33"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a53aa091-d475-4f4e-bc7c-20361c476a29"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a54f2a19-1469-40d9-8913-799107453654"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a5aa2617-09b7-4889-a863-2dfa6d254236"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a5f820dd-37d5-4ef6-88d1-d779183c306b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a63548af-fe08-47cf-acd0-d9ea702b4e0c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a661eb9f-262c-49e8-bc53-1010973021ab"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a6c371f6-a52e-4821-bb68-c59e7f0a3b1e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a6ee57cb-dda6-4d29-852c-62b762d147a1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a6f34849-54f7-44e7-8fce-265a9a8b604f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a72c02db-34ae-4be2-96eb-7ed3f992499d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a7311eba-644f-436f-ba95-dbdd124d999a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a74eda3a-bf03-49f6-a1bb-4ba277d6336b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a7ecceeb-ea05-42e8-a6ef-0119e309c5a6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a82e9405-d427-4db3-9a4b-5292eb76f31e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a8853a77-64b4-4eea-b3a1-017d2309d8b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a8ea4caa-1e57-4a50-9817-c718dad73539"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("a9820956-84cd-4373-8e93-04eb01c45c87"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aa0aa029-25ab-41ee-9414-46b1c6d6c238"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aa2806f9-ea36-48b1-a5b2-04fee3f81a0d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aa778a08-5ad9-4268-b8c0-f457c8b591ab"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("abe80c99-0bbf-4e48-933b-874b65358eca"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ac1f4e4a-6f25-4349-b7a3-8966bd6ac2db"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ac84273e-bbe7-4720-b731-5892e4ee1b2c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("acf1a783-686a-4ef2-a3ca-0e87a7006541"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ae1bc8c1-6025-455b-900d-9476965e1904"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ae74328a-9f61-4d1e-9247-4895ae7045da"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("aeb507d7-b0fc-466c-a26a-c2b4165bd01a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("af1a9b2b-5a89-4b4b-a434-d58e123afc32"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("af29e41e-2f82-41ac-983e-ff1df2294e58"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b033556e-8bd5-4277-bf79-9875774dc403"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b0dd2ccd-a351-4621-92e7-8adacb8e55b4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b0ff6cdd-9485-4e2f-b7cb-40c5161d631b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b13452a4-becf-4e6c-8edf-2e6995b4c5a5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b1b3cdb9-8e54-4e1b-b94c-9cbe63d3d86a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b1ce8793-4601-44ff-b71b-fdd3ccec7cfd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b284d100-3ee7-4f46-873d-3f75cf0f1540"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b2a38a25-3fd9-4274-8ee5-f85744670ef1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b2b3ca4a-fa44-4040-8ee5-44b05a56628c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b2efe4a8-51f0-452b-9a9b-6b6d4ce1ca69"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b3055ec0-0a3f-45d1-bba1-4bbd43aae087"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b30f30f7-51e8-4805-b1d3-6d719a0b5b9a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b50b66ed-8182-4ef8-9b7d-848f14cb3e91"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b5cee958-0e64-41a4-9f66-bc008c7a59e9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b6d8ea7c-1d31-4102-baf2-a799480ca577"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b700b31e-6d6d-4ee5-90c7-5680b8cf34d7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b74e4e2f-f51f-4fd3-8167-9ad2c8e588eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b787c819-9215-4757-80e6-5349c10d1564"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b7a478b2-102b-4f92-870e-4ed979c162e4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b7a95499-6115-4c97-ba9f-8f2fbef86169"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b9262d3f-b00c-47e4-a6fb-7891523fce51"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b95f2b70-0a48-4470-bf16-8ba6fc1a2264"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ba9b7f24-7e91-42ca-abfe-ce530f26aa36"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bb4484b3-b0ca-4e63-88f6-cb84192835b2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc13b36e-d30d-4795-aa0e-d0a69894534c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc50c79a-5248-4b84-b4b7-027b2c59371d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc5a7dc6-be8a-47f3-b02e-5b2efe6a379e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bc9d011b-b150-4d67-843f-d52fcc7c026a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bd24c256-a61d-4c8e-b656-0f90bd2cff3d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bd42b32e-820f-4961-a732-5e3bcb953f3f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bd77d4f6-5a7e-48e4-a4fb-6c2e6bb41a60"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("be836647-61ad-4068-824d-06198752c7e0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("becd2e23-8d27-42b8-9b03-d96571023580"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bfa31f50-c61d-4ff9-b92a-193242ebf89e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bfe242b1-00bb-4e30-a0a8-9614bbd2fd52"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c0185c82-7014-4a10-85a5-a644cbf4572b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c07e2713-e938-4f52-92ff-ef19624eb235"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c109b0b2-33f3-4d29-8d37-e25623d9a2f6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c1a0da74-9e02-43fc-beec-14589057478c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c1c90529-f7d0-40d0-b644-9c15e3046c25"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c267a057-4558-4f6a-aa42-3a034e868d64"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c2769933-d4a2-4571-90a3-d624c0ab6c70"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c2e2f1fe-7a7e-4f18-8c45-869a53dd6124"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c2e5388f-a002-4e74-b924-ae34eb7fd6b0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c39362be-0a0c-440a-a471-5daa4d41b33a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c3bf9ab5-a70f-425d-9c61-4d270ae5a80c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c3d08dd5-1216-4448-b62c-a0d2ca781294"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c4460d63-44e4-412c-8de9-b052514e41b5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c562c7b6-2cf8-4ec1-8cb2-2fd0d40169aa"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c58dfe16-bcf6-4380-97e2-a2250c63b41d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c62075a8-2590-4623-a907-43d3109f6139"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c6b597dd-0f08-47b1-95a9-2aba06df4db9"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c6f1ace8-1305-4cd6-a1ad-db34fca5105f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c717489d-8ca6-4da9-91fa-f22bc9cd7427"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c72ae942-1baa-4136-a21f-39438bf52f71"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c80bb3a7-eef4-48a6-aeed-a3e6ea055b17"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c88a545c-fe4a-4bff-ab44-0f00f40e721f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c910f3d0-3549-4ae8-be9b-3a1ee795b22e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c9235f5d-775f-40de-aa3b-c9dec5678d73"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cad2c172-1c1b-4194-81ee-582deef55f14"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cb7c01c0-f60e-4071-b83f-3520279d6c4c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cc2a4a36-92dd-4ac1-97ee-9648940a0dea"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cc66f1f9-f212-4007-9797-bc61d23eb847"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ccb52a7d-4fa7-40e9-a737-67122b16f974"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cdfa84d5-e31d-4920-bff0-53cbc78142f4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cebbe2c3-f6e8-49a9-adfc-7264fe6f0539"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ced155c4-7eff-4c5e-8b38-1123d717d0bd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cf00498b-dbb6-48f1-83ec-aae4572daad4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cf060c72-425b-47ac-a892-891658b59481"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("cf398c9f-2fb9-4f22-a5a7-21687b670fb0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d016bc16-ed2e-43cd-837d-8867a4435c99"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d0424f82-f884-450e-b259-821f08f1c29f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d059dd3f-7ee1-4fd3-bdd1-56034e08e99f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d0602bd5-530d-474e-bf74-532bfbfeb6d8"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d1de4314-6221-4270-a11f-4c230da0c71d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d20f8b13-6717-47ca-a235-f772a8d16514"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d21489d6-7d1e-46fb-9d11-c4b536285347"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d2207ecd-576b-43b9-9a26-bbe647c96c0a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d2558cd1-71ca-4c7f-802d-98498b2eaf90"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d2563ac7-9d6e-428f-8b4e-6cd1254f2612"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d27fe1ae-3758-48db-9da9-7c0ea3c6c5ca"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d30ce642-60ee-4c35-943e-3a5ca2075b72"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d383e248-a391-4cad-8394-85871b372412"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d43a3585-c851-48ae-9ffd-8cf9045befc2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d586409a-1bf3-4b39-8d0e-7ca0537d1cef"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d5b0e5e7-051f-4f72-9980-288125b680f1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d5e9268f-e45e-48cd-aa4d-35a846fecb45"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d66633d4-3d4a-435c-a8ae-90845e79a6e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d6bf4347-d6c1-49c2-9eac-f3cb57e082cc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d6d3ad1b-1470-4840-bbd4-1127d47295cd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d719e975-644b-4fda-b001-c77ab132a672"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d7d5c158-2e6f-4133-a601-14d729efb1f5"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d8705a5e-3662-406d-a540-afa447aabca3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d907b8c5-ea96-4ded-a696-5e906b2537bc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db2877fd-2b4b-402e-9964-af271f1cd9ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db50fe9a-3561-4848-8aa7-f04ef2f69fa2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db51d970-e418-4755-b7cd-beedeb36137f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db5d0848-c1cd-4e30-a28c-a6199621174c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("db5ec265-0e69-4987-9f3b-8632840fe10e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dbd45055-c1c6-46f5-828f-392e9c9ba04b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dbf3318e-ce56-4ff7-b3d5-ef84c846b9eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dc26d176-1277-4ce2-a1a9-4ccadd053e27"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dc37346d-f34c-451b-90f0-35b5d69aae46"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dc90bc15-6ebe-4d24-b9d1-d1d2b2ace8eb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dd30aad5-4955-43fc-806e-3700ed14257e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("dd42651a-effa-460a-b5bc-3401bf75a628"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("de169d34-4dc6-4a05-87eb-20901416dafb"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("de3ccd70-38df-460b-9a20-e0857c55c292"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("de747d91-aa94-411e-a7c0-55873c96ad2c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e0fd498c-85f1-49d2-a283-0dcc0dc367b6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e1e68b14-02cc-40a2-b7db-0f3ec16ecc89"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e251a3ab-ebcf-4128-a93d-aefb37b39fda"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e26d81f7-1557-4193-bc39-0edb829e0f37"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e310a5bd-842e-4298-a162-667abb8e9dd2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e36442ed-35cb-40e0-882c-1c5d41ba3f68"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e3792b8f-19f7-4ae1-ad55-43655ae27012"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e59daa5f-b8f2-441d-b116-9a5eb1ba05cd"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e5afd154-c762-434b-ac16-a54d71737d31"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e738594b-19ef-4a88-9dbf-a5a7a5ff443a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e78d3e47-1f78-4873-bef0-105912a4a28a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e78dc003-3cb5-4af6-bac6-822748137109"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e798fcd0-c648-4a84-aa52-4274711438e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e7cf1925-02b2-4761-a526-98733667e908"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e8324d8b-03ab-47c6-8385-514fac6dca6a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e83fcec2-0045-4e25-b86d-95451479d08b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e883218f-e410-4256-aa98-47be80158122"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e88b61ba-4d65-4a7c-b7b2-833a86472322"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e9241b59-41a5-463a-8933-2abe381d91b6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("e9ca976c-fa94-4525-9eaa-f8f4f550249e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ea38954b-b569-46b4-ac8f-e36a236eb4d2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ead4dc97-a84a-4deb-a1ce-73cf295a0095"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ecad63ff-94da-48ff-927a-db56bff7b4e7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eda85b84-4400-49fe-a011-cc06ea57aee3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ee749b92-c917-46da-adc3-b2a67faa98ed"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eea9a534-f4b5-457b-a8bb-73a2d40330b6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eeaeea6e-08c1-4096-9866-588752a91764"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("eeb163a4-245b-403d-838c-f03b4e195b5c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f025f44d-515c-4e9c-aabe-e914d94e6694"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f025fbe2-464a-4f53-aa78-6e02c5c116b3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f0ac5cc9-186e-4854-b103-98b515e91eac"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f0d93c98-3a4d-4efd-90b6-70d7cfb25617"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f11294d0-1f00-4582-9d24-3707bf5280d6"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f11f97b1-3e83-4c7a-a5ef-06c1846ce98e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f128c767-8664-40b9-a086-fd929ae133a2"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f13b200d-fd21-4078-a951-fc5fc331718d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f143cc8c-b3bd-4225-a0ba-2ecfcd46144c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f1484bb2-5277-45f9-8549-0c4ea1255aff"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f16b7c49-8180-438f-a114-9a2e848cac52"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f261f4c6-1812-48a1-851f-90e88cac5ddc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f26b852b-1da4-443b-a062-44696556e5f3"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f3083498-736f-421a-a001-feb4cf951422"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f370618c-e33a-4663-90fc-7c986e4053ae"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f376d84f-37b1-4013-bb85-209788983603"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f5326f7f-d87f-4f89-b7c2-36220872fcaf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f572f90a-512c-4271-825b-ef6c82c28610"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f5d814fc-d3a5-4b3b-af84-2895a05815f1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f6558ad2-1df5-4d80-8f5d-17fa2235b97b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f688b1b4-7f21-42ef-b23e-9d69ea97cd05"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f6af9dae-a163-419f-ade9-d4231621e518"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f7d4b02a-494c-466e-b7d6-9a11cb09267a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f81364f2-4002-4d20-8728-3997c24a62b4"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f835d1ab-0e33-4291-b567-60aac42f8c6e"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f8d1338d-9db8-4866-b060-c3ca4a7a3d08"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f8eeccf0-529d-4a86-ac0a-8795a315b33a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f934b137-6e22-4dc8-bcc4-272f75d7f6df"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f937f259-3e02-4207-b62f-5528bc469424"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f9454382-9fbd-4524-8b65-04c1b449469c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f98655e6-a6cc-4759-8e6f-5bb39dd74403"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f99825d4-8949-42bd-af1c-3ad4eb9b14e1"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fa7f0d01-6347-41c3-b183-e504eb17cc6c"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fa8fcdc3-4477-4b51-afc6-1deb64bf99fe"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fb02a43a-455a-4e3c-a16e-3799bfcf6312"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fbf5eed1-36ed-49ef-8205-89fc822b3f28"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fc53ef0a-c36f-46d9-b6f0-f8f86cd9cb00"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fc9372fa-0d90-4257-9b2f-e4a80aaee992"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fcaec936-3aff-4b19-a1a0-048ea2a06ae0"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fcecc812-b678-42b4-9273-d7766014e515"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fd8337ea-cb3f-46b0-8586-fdd9dbd64862"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fd883e81-6f40-4d86-a7b0-776a1b39bc75"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fe09a91e-aa63-47b0-9464-b00d32ebfe54"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fe6a5f2d-0389-45de-8391-56d5b0f65374"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("ff098a16-6549-4b31-ae3f-e23f5d68713e"));

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0424");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0425");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0426");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0427");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0428");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0429");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0430");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0431");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0432");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0433");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0434");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0435");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0436");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0437");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0438");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0439");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0440");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0441");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0442");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0443");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0444");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0445");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0446");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0447");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0448");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0449");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0450");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0451");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0452");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0453");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0454");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0455");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0456");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0457");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0458");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0459");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0460");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0461");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0462");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0463");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0464");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0465");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0466");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0467");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0468");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0469");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0470");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0471");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0472");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0473");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0474");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0475");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0476");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0477");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0478");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0479");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0480");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0481");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0482");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0483");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0484");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0485");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0486");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0487");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0488");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0489");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0490");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0491");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0492");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0493");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0494");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0495");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0496");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0497");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0498");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0499");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0500");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0501");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0502");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0503");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0504");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0505");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0506");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0507");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0508");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0509");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0510");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0511");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0512");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0513");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0514");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0515");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0516");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0517");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0518");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0519");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0520");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0521");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0522");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0523");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0524");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0525");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0526");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0527");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0528");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0529");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0530");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0531");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0532");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0533");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0534");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0535");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0536");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0537");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0538");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0539");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0540");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0541");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0542");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0543");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0544");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0545");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0546");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0547");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0548");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0549");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0550");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0551");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0552");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0553");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0554");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0555");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0556");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0557");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0558");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0559");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0560");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0561");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0562");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0563");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0564");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0565");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0566");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0567");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0568");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0569");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0570");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0571");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0572");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0573");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0574");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0575");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0576");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0577");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0578");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0579");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0580");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0581");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0582");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0583");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0584");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0585");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0586");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0587");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0588");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0589");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0590");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0591");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0592");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0593");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0594");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0595");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0596");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0597");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0598");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0599");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0600");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0601");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0602");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0603");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0604");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0605");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0606");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0607");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0608");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0609");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0610");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0611");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0612");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0613");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0614");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0615");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0616");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0617");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0618");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0619");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0620");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0621");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0622");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0623");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0624");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0625");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0626");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0627");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0628");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0629");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0630");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0631");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0632");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0633");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0634");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0635");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0636");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0637");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0638");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0639");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0640");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0641");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0642");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0643");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0644");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0645");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0646");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0647");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0648");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0649");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0650");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0651");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0652");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0653");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0654");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0655");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0656");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0657");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0658");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0659");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0660");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0661");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0662");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0663");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0664");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0665");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0666");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0667");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0668");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0669");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0670");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0671");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0672");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0673");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0674");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0675");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0676");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0677");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0678");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0679");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0680");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0681");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0682");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0683");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0684");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0685");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0686");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0687");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0688");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0689");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0690");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0691");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0692");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0693");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0694");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0695");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0696");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0697");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0698");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0699");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0700");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0701");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0702");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0703");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0704");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0705");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0706");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0707");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0708");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0709");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0710");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0711");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0712");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0713");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0714");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0715");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0716");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0717");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0718");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0719");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0720");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0721");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0722");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0723");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0724");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0725");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0726");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0727");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0728");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0729");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0730");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0731");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0732");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0733");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0734");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0735");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0736");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0737");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0738");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0739");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0740");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0741");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0742");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0743");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0744");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0745");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0746");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0747");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0748");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0749");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0750");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0751");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0752");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0753");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0754");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0755");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0756");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0757");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0758");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0759");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0760");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0761");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0762");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0763");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0764");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0765");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0766");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0767");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0768");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0769");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0770");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0771");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0772");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0773");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0774");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0775");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0776");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0777");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0778");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0779");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0780");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0781");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0782");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0783");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0784");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0785");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0786");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0787");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0788");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0789");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0790");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0791");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0792");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0793");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0794");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0795");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0796");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0797");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0798");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0799");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0800");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0801");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0802");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0803");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0804");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0805");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0806");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0807");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0808");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0809");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0810");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0811");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0812");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0813");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0814");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0815");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0816");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0817");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0818");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0819");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0820");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0821");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0822");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0823");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0824");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0825");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0826");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0827");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0828");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0829");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0830");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0831");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0832");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0833");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0834");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0835");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0836");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0837");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0838");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0839");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0840");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0841");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0842");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0843");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0844");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0845");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0846");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0847");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0848");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0849");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0850");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0851");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0852");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0853");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0854");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0855");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0856");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0857");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0858");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0859");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0860");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0861");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0862");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0863");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0864");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0865");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0866");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0867");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0868");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0869");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0870");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0871");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0872");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0873");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0874");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0875");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0876");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0877");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0878");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0879");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0880");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0881");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0882");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0883");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0884");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0885");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0886");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0887");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0888");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0889");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0890");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0891");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0892");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0893");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0894");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0895");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0896");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0897");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0898");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0899");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0900");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0901");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0902");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0903");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0904");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0905");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0906");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0907");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0908");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0909");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0910");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0911");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0912");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0913");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0914");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0915");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0916");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0917");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0918");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0919");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0920");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0921");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0922");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0923");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0924");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0925");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0926");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0927");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0928");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0929");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0930");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0931");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0932");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0933");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0934");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0935");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0936");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0937");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0938");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0939");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0940");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0941");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0942");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0943");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0944");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0945");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0946");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0947");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0948");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0949");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0950");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0951");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0952");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0953");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0954");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0955");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0956");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0957");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0958");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0959");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0960");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0961");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0962");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0963");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0964");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0965");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0966");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0967");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0968");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0969");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0970");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0971");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0972");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0973");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0974");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0975");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0976");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0977");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0978");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0979");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0980");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0981");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0982");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0983");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0984");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0985");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0986");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0987");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0988");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0989");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0990");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0991");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0992");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0993");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0994");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0995");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0996");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0997");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0998");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0999");

            migrationBuilder.DropColumn(
                name: "IsLegacy",
                table: "TbService");
        }
    }
}
