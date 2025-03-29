<template>
  <div class="code-generator">
    <h2>代码生成器</h2>
    <p>快速生成前后端代码，提升开发效率</p>
    
    <div class="card">
      <div class="form-item">
        <div class="label">数据表字段</div>
        <el-select
          v-model="selectedTables"
          multiple
          filterable
          placeholder="请选择字段"
          class="select-tables"
        >
          <el-option
            v-for="table in tables"
            :key="table"
            :label="table"
            :value="table"
          />
        </el-select>
      </div>
      
      <div class="options-container">
        <div class="option-group">
          <div class="label">后端代码生成选项</div>
          <el-checkbox-group v-model="backendOptions">
            <el-checkbox label="Model">Model</el-checkbox>
            <el-checkbox label="BLL">BLL</el-checkbox>
            <el-checkbox label="Controllers">Controllers</el-checkbox>
          </el-checkbox-group>
        </div>
        
        <div class="option-group">
          <div class="label">前端代码生成选项</div>
          <el-checkbox-group v-model="frontendOptions">
            <el-checkbox label="Access">Access</el-checkbox>
            <el-checkbox label="API">API</el-checkbox>
            <el-checkbox label="Views">Views</el-checkbox>
          </el-checkbox-group>
        </div>
      </div>
      
      <div class="action-button">
        <el-button type="primary" @click="generateCode">
          <i class="el-icon-download"></i>
          生成代码
        </el-button>
      </div>
    </div>
    
    <div class="search-bar">
      <el-input
        v-model="searchKeyword"
        placeholder="搜索..."
        prefix-icon="el-icon-search"
        clearable
      />
      <el-select v-model="pageSize" class="page-size-select">
        <el-option label="10条/页" value="10" />
        <el-option label="20条/页" value="20" />
        <el-option label="50条/页" value="50" />
      </el-select>
    </div>
    
    <el-table :data="generatedCodes" class="result-table">
      <el-table-column prop="tableName" label="表" />
      <el-table-column prop="code" label="代码" />
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
      <span class="total-info">Total {{ total }}</span>
      <el-select v-model="pageSize" size="small" class="page-size-select">
        <el-option label="10/page" value="10" />
      </el-select>
      <el-pagination
        :current-page="currentPage"
        :page-size="pageSize"
        layout="prev, pager, next, jumper"
        :total="total"
        @current-change="handlePageChange"
      />
      <span class="go-to">Go to</span>
      <el-input v-model="goToPage" size="mini" class="go-to-input" @keyup.enter="handleGoTo" />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
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
  total: number;
  items: T[];
}

const tables = ref<string[]>([]);
const selectedTables = ref<string[]>([]);
const generatedCodes = ref<GeneratedCodeResult[]>([]);
const currentPage = ref(1);
const pageSize = ref('10');
const total = ref(0);
const backendOptions = ref<string[]>([]);
const frontendOptions = ref<string[]>([]);
const searchKeyword = ref('');
const goToPage = ref('');

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
    const res = await request<DataRes<PageResult<GeneratedCodeResult>>>({
      url: '/api/codegen/codes',
      method: 'get',
      params: {
        page: currentPage.value,
        size: parseInt(pageSize.value),
        keyword: searchKeyword.value
      }
    });
    
    if (res.code === 200) {
      generatedCodes.value = res.data.items;
      total.value = res.data.total;
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

const handleGoTo = () => {
  const page = parseInt(goToPage.value);
  if (!isNaN(page) && page > 0 && page <= Math.ceil(total.value / parseInt(pageSize.value))) {
    currentPage.value = page;
    fetchGeneratedCode();
  }
  goToPage.value = '';
};

onMounted(() => {
  fetchTables();
  //fetchGeneratedCode();
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

.options-container {
  display: flex;
  margin-bottom: 20px;
}

.option-group {
  flex: 1;
  padding-right: 20px;
}

.action-button {
  text-align: right;
}

.search-bar {
  display: flex;
  justify-content: space-between;
  margin-bottom: 15px;
}

.search-bar .el-input {
  width: 240px;
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
</style>