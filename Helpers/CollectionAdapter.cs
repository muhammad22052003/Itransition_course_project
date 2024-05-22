using CourseProject_backend.Entities;

namespace CourseProject_backend.Helpers
{
    public class CollectionAdapter
    {
        public List<List<string>> AdapteToListTable(MyCollection collection)
        {
            List<List<string>> dataTable = new List<List<string>>();

            dataTable.Add(new List<string>());

            dataTable[0].Add("Item Id");

            dataTable[0].Add("Item Name");

            if (collection.CustomText1_state) { dataTable[0].Add(collection.CustomText1_name); }
            if (collection.CustomText2_state) { dataTable[0].Add(collection.CustomText2_name); }
            if (collection.CustomText3_state) { dataTable[0].Add(collection.CustomText3_name); }

            if (collection.CustomString1_state) { dataTable[0].Add(collection.CustomString1_name); }
            if (collection.CustomString2_state) { dataTable[0].Add(collection.CustomString2_name); }
            if (collection.CustomString3_state) { dataTable[0].Add(collection.CustomString3_name); }

            if (collection.CustomInt1_state) { dataTable[0].Add(collection.CustomInt1_name); }
            if (collection.CustomInt2_state) { dataTable[0].Add(collection.CustomInt2_name); }
            if (collection.CustomInt3_state) { dataTable[0].Add(collection.CustomInt3_name); }

            if (collection.CustomBool1_state) { dataTable[0].Add(collection.CustomBool1_name); }
            if (collection.CustomBool2_state) { dataTable[0].Add(collection.CustomBool2_name); }
            if (collection.CustomBool3_state) { dataTable[0].Add(collection.CustomBool3_name); }

            if (collection.CustomDate1_state) { dataTable[0].Add(collection.CustomDate1_name); }
            if (collection.CustomDate2_state) { dataTable[0].Add(collection.CustomDate2_name); }
            if (collection.CustomDate3_state) { dataTable[0].Add(collection.CustomDate3_name); }

            var items = collection.Items.ToArray();

            for (int i = 0; i < collection.Items.Count; i++)
            {
                dataTable.Add(new List<string>());

                dataTable[i + 1].Add(items[i].Id);

                dataTable[i + 1].Add(items[i].Name);

                if (collection.CustomText1_state) { dataTable[i+1].Add(items[i].CustomText1); }
                if (collection.CustomText2_state) { dataTable[i + 1].Add(items[i].CustomText2); }
                if (collection.CustomText3_state) { dataTable[i + 1].Add(items[i].CustomText3); }

                if (collection.CustomString1_state) { dataTable[i + 1].Add(items[i].CustomString1); }
                if (collection.CustomString2_state) { dataTable[i + 1].Add(items[i].CustomString2); }
                if (collection.CustomString3_state) { dataTable[i + 1].Add(items[i].CustomString3); }

                if (collection.CustomInt1_state) { dataTable[i + 1].Add(items[i].CustomInt1.ToString()); }
                if (collection.CustomInt2_state) { dataTable[i + 1].Add(items[i].CustomInt2.ToString()); }
                if (collection.CustomInt3_state) { dataTable[i + 1].Add(items[i].CustomInt3.ToString()); }

                if (collection.CustomBool1_state) { dataTable[i + 1].Add(items[i].CustomBool1.ToString()); }
                if (collection.CustomBool2_state) { dataTable[i + 1].Add(items[i].CustomBool2.ToString()); }
                if (collection.CustomBool3_state) { dataTable[i].Add(items[i].CustomBool3.ToString()); }

                if (collection.CustomDate1_state) { dataTable[i + 1].Add(items[i].CustomDate1?.Date.ToShortDateString()); }
                if (collection.CustomDate2_state) { dataTable[i + 1].Add(items[i].CustomDate2?.Date.ToShortDateString()); }
                if (collection.CustomDate3_state) { dataTable[i + 1].Add(items[i].CustomDate3?.Date.ToShortDateString()); }
            }

            return dataTable;
        }
    }
}
