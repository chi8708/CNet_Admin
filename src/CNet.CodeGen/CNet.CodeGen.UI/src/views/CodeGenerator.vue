<template>
  <div class="code-generator">
    <h2>代码生成器</h2>
    <p>快速生成前后端代码，提升开发效率</p>

    <div class="card">
      <div class="form-item">
        <div class="label">数据表字段</div>
        <el-select v-model="selectedTables" multiple filterable placeholder="请选择字段" class="select-tables">
          <el-option v-for="table in tables" :key="table" :label="table" :value="table" />
        </el-select>
      </div>

      <div class="options-row">
        <div class="option-label">后端替换选项</div>
        <el-checkbox v-model="backendSelectAll" @change="handleBackendSelectAllChange" class="select-all-checkbox">全选</el-checkbox>
        <el-checkbox-group v-model="backendOptions" @change="handleBackendOptionsChange" class="checkbox-group">
          <el-checkbox label="Model">Model</el-checkbox>
          <el-checkbox label="BLL">BLL</el-checkbox>
          <el-checkbox label="Controllers">Controllers</el-checkbox>
        </el-checkbox-group>
        
        <div class="option-label frontend-label">前端替换选项</div>
        <el-checkbox v-model="frontendSelectAll" @change="handleFrontendSelectAllChange" class="select-all-checkbox">全选</el-checkbox>
        <el-checkbox-group v-model="frontendOptions" @change="handleFrontendOptionsChange" class="checkbox-group">
          <el-checkbox label="Access">Access</el-checkbox>
          <el-checkbox label="API">API</el-checkbox>
          <el-checkbox label="Views">Views</el-checkbox>
        </el-checkbox-group>
        
        <div class="action-button">
          <el-button type="primary" @click="generateCode">
            <i class="el-icon-download"></i>
            生成代码
          </el-button>
        </div>
      </div>
    </div>


    <!-- 添加带边框的生成记录区域 -->
    <div class="records-container">
      <div class="records-header">
        <h3>生成记录</h3>
      </div>
      <div class="search-bar">
        <el-input v-model="queryData.SL_TableName" placeholder="搜索表名..." prefix-icon="el-icon-search" clearable
          @keyup.enter="fetchGeneratedCode" />
        <el-button type="primary" @click="fetchGeneratedCode">搜索</el-button>
      </div>
      <el-table :data="generatedCodes" class="result-table">
        <el-table-column prop="tableName" label="表" />
        <el-table-column label="生成时间">
          <template #default="scope">
            {{ formatDateTime(scope.row.createTime) }}
          </template>
        </el-table-column>
        <el-table-column prop="genInfo" label="替换项" />
        <el-table-column label="操作" width="200">
          <template #default="scope">
            <div class="table-actions">
              <el-button type="primary" size="small" @click="viewCode(scope.row)">
                查看
              </el-button>
              <el-button type="success" size="small" @click="downloadCode(scope.row)">
                下载
              </el-button>
            </div>
          </template>
        </el-table-column>
      </el-table>

      <div class="pagination-container">
        <span class="total-info">总数 {{ total }}条</span>
        <span class="total-info">页数 {{ totalPage }}页</span>
        <el-select v-model="pageSize" class="page-size-select" @change="handlePageSizeChange">
          <el-option label="10条/页" :value="10" />
          <el-option label="20条/页" :value="20" />
          <el-option label="50条/页" :value="50" />
        </el-select>
        <el-pagination :current-page="currentPage" :page-size="pageSize" :total="total"
          layout="prev, pager, next, jumper" @current-change="handlePageChange" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, reactive } from 'vue';
import { ElMessage } from 'element-plus';
import request from '@/utils/request';

// 定义接口返回数据类型
interface DataRes<T> {
  code: number;
  msg: string;
  data: T;
}

// 定义生成代码结果类型
interface GeneratedCodeResult {
  tableName: string;
  code: string;
  generatedTime: string;
}

// 定义分页结果类型
interface PageResult<T> {
  code: number
  msg: string;
  total: number;
  data: T[]
}

const tables = ref<string[]>([]);
const selectedTables = ref<string[]>([]);
const generatedCodes = ref<GeneratedCodeResult[]>([]);
const currentPage = ref(1);
const pageSize = ref(10);
const total = ref(0);
const totalPage = ref(0);
const backendSelectAll = ref(true);
const frontendSelectAll = ref(false);
const backendOptions = ref(['Model', 'BLL', 'Controllers']);
const frontendOptions = ref([]);
const searchKeyword = ref('');
const goToPage = ref('');
const queryData =reactive({
  SL_TableName: ''
});
const fetchTables = async () => {
  try {
    const res = await request<DataRes<string[]>>({
      url: '/api/codegen/tables',
      method: 'get'
    });

    if (res.code === 200) {
      tables.value = res.data;
    } else {
      ElMessage.error(res.msg || '获取数据表失败');
    }
  } catch (error) {
    ElMessage.error('获取数据表失败');
  }
};

const generateCode = async () => {
  if (selectedTables.value.length === 0) {
    ElMessage.warning('请至少选择一个数据表');
    return;
  }

  try {
    const res = await request<DataRes<any>>({
      url: '/api/codegen/generate',
      method: 'post',
      data: {
        tables: selectedTables.value,
        options: {
          backend: {
            model: backendOptions.value.includes('Model'),
            bll: backendOptions.value.includes('BLL'),
            controllers: backendOptions.value.includes('Controllers')
          },
          frontend: {
            access: frontendOptions.value.includes('Access'),
            api: frontendOptions.value.includes('API'),
            views: frontendOptions.value.includes('Views')
          }
        }
      }
    });

    if (res.code === 200) {
      ElMessage.success('代码生成成功');
      fetchGeneratedCode();
    } else {
      ElMessage.error(res.msg || '代码生成失败');
    }
  } catch (error) {
    ElMessage.error('代码生成失败');
  }
};

const fetchGeneratedCode = async () => {
  try {
    const res = await request<PageResult<GeneratedCodeResult>>({
      url: '/api/codegen/GetPage',
      method: 'post',
      data: {
        pageNum: currentPage.value,
        pageSize: pageSize.value,
        field: "Id",
        order: "desc",
        query: queryData
      }
    });

    if (res.code === 1) {
      generatedCodes.value = res.data;
      total.value = res.count || 0;
      totalPage.value = res.totalPage;
    } else {
      ElMessage.error(res.msg || '获取生成的代码列表失败');
    }
  } catch (error) {
    ElMessage.error('获取生成的代码列表失败');
  }
};

interface CodeContent {
  access: string;
  api: string;
  view_list: string;
  view_edit: string;
  model: string;
  bll: string;
  dal: string;
}

const viewCode = async (row: GeneratedCodeResult) => {
  try {
    const res = await request<DataRes<CodeContent>>({
      url: `/api/codegen/view/${row.tableName}`,
      method: 'get'
    });

    if (res.code === 200) {
      // 处理查看代码的逻辑
      console.log(res.data);
      // 这里可以添加显示代码的弹窗逻辑
    } else {
      ElMessage.error(res.msg || '获取代码内容失败');
    }
  } catch (error) {
    ElMessage.error('获取代码内容失败');
  }
};

const downloadCode = (row: GeneratedCodeResult) => {
  // 使用浏览器下载功能
  window.location.href = `/api/codegen/download/${row.tableName}`;
};

const handlePageChange = (page: number) => {
  currentPage.value = page;
  fetchGeneratedCode();
};

const handlePageSizeChange = () => {
  currentPage.value = 1;
  fetchGeneratedCode();
};

// 格式化日期时间，去除T
const formatDateTime = (dateTimeStr: string) => {
  if (!dateTimeStr) return '';
  return dateTimeStr.replace('T', ' ');
};

// 后端选项全选
const handleBackendSelectAllChange = (val: boolean) => {
  backendOptions.value = val ? ['Model', 'BLL', 'Controllers'] : [];
};

// 前端选项全选
const handleFrontendSelectAllChange = (val: boolean) => {
  frontendOptions.value = val ? ['Access', 'API', 'Views'] : [];
};

// 根据选择情况更新全选状态
const handleBackendOptionsChange = (value: string[]) => {
  const allBackendOptions = ['Model', 'BLL', 'Controllers'];
  backendSelectAll.value = value.length === allBackendOptions.length;
};

// 根据选择情况更新全选状态
const handleFrontendOptionsChange = (value: string[]) => {
  const allFrontendOptions = ['Access', 'API', 'Views'];
  frontendSelectAll.value = value.length === allFrontendOptions.length;
};

onMounted(() => {
  fetchTables();
  fetchGeneratedCode();
});
</script>

<style scoped>
.code-generator {
  padding: 20px;
  font-size: 14px;
}

.card {
  background-color: #fff;
  border-radius: 4px;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.1);
  padding: 20px;
  margin-bottom: 20px;
}

.form-item {
  margin-bottom: 20px;
}

.label {
  font-size: 14px;
  color: #606266;
  margin-bottom: 10px;
}

.select-tables {
  width: 100%;
}

.options-row {
  display: flex;
  align-items: center;
  margin-bottom: 15px;
  flex-wrap: nowrap;
}

.option-label {
  margin-right: 10px;
  white-space: nowrap;
}

.frontend-label {
  margin-left: 20px;
}

.select-all-checkbox {
  margin-right: 10px;
}

.checkbox-group {
  display: flex;
  margin-right: 15px;
}

.checkbox-group .el-checkbox {
  margin-right: 10px;
  margin-bottom: 0;
}

.action-button {
  margin-left: auto;
}

.search-bar {
  display: flex;
  justify-content: space-between;
  margin-bottom: 15px;
  gap: 10px;
}

.search-bar .el-input {
  flex: 1;
}

.page-size-select {
  width: 100px;
  margin-left: 10px;
}

.result-table {
  width: 100%;
  margin-bottom: 15px;
}

.table-actions {
  display: flex;
  gap: 10px;
}

.pagination-container {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  font-size: 13px;
}

.total-info {
  margin-right: 10px;
}

.go-to {
  margin: 0 10px;
}

.go-to-input {
  width: 50px;
}

.records-container {
  border: 1px solid #e4e7ed;
  border-radius: 4px;
  padding: 20px;
  margin-bottom: 20px;
  background-color: #fff;
}

.records-header {
  margin-bottom: 15px;
  border-bottom: 1px solid #ebeef5;
  padding-bottom: 10px;
}

.records-header h3 {
  margin: 0;
  font-size: 16px;
  font-weight: 500;
  color: #303133;
}
</style>